using System;

public class StringToDNA
{

    public static void Main()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Enter the number of the Objective you wish to test (1-5), or (q)uit.");
            string userCommand = Console.ReadLine();
            switch (userCommand[0])
            {
                case '1':
                    obj1and2();
                    break;
                case '2':
                    goto case '1';
                case '3':
                    obj3();
                    break;
                case '4':
                    obj4();
                    break;
                case '5':
                    obj5();
                    break;
                case 'q':
                    running = false;
                    break;
                default:
                    continue;
            }
        }
    }

    //routines corresponding to their objectives
    private static void obj1and2()
    {
        Console.WriteLine("Enter an ASCII string to encode with DNA/RNA");
        string fromUser = Console.ReadLine();
        Console.WriteLine("Would you like the output to be in RNA?(y/n)");
        string wantsRNA = Console.ReadLine();
        bool isRNA = wantsRNA.Contains("y");
        string res = stringToDNA(fromUser, isRNA);
        Console.WriteLine(String.Format("\t{0} version:{1}", isRNA ? "RNA" : "DNA", res));
    }
    private static void obj3()
    {
        Console.WriteLine("Enter a string to check for DNA");
        string fromUser = Console.ReadLine();
        int index = firstIndexOfDNA(fromUser);
        Console.WriteLine("\tFirst index of DNA sequence: " + index);
    }
    private static void obj4()
    {
        Console.WriteLine("Enter a DNA complement strand(ALL CAPS)");
        string fromUser = Console.ReadLine();
        string result = complementDNAToASCII(fromUser);
        Console.WriteLine("\tdecoded into ASCII: " + result);
    }
    private static void obj5()
    {
        Console.WriteLine("Enter DNA strand 1 (ALL CAPS)");
        string strand1 = Console.ReadLine();
        Console.WriteLine("Enter DNA strand 2");
        string strand2 = Console.ReadLine();
        string lcs = longestCommonSubseq(strand1, strand2);
        Console.WriteLine("\tTheir longest common subsequence is: " + lcs);
    }


    //convert one ascii character to DNA
    //(objectives 1 & 2)
    private static string charToDNA(char ascii, bool isRNA)
    {
        string result = "";
        int asciiValue = (int)ascii;//get [0,255] value of input
        for (int i = asciiValue; i > 0; i /= 4)
        {
            int currentDigit = i % 4;
            switch (currentDigit)
            {
                case 0:
                    result = "A" + result;
                    break;
                case 1:
                    result = isRNA ? "U" + result : "T" + result;
                    break;
                case 2:
                    result = "G" + result;
                    break;
                case 3:
                    result = "C" + result;
                    break;
            }
        }

        return result;
    }

    //convert ascii string to DNA/RNA string
    //(objective 1)
    public static string stringToDNA(string ascii, bool isRNA)
    {
        string result = "";
        foreach (char a in ascii)
        {
            result += charToDNA(a, isRNA);
        }
        return result;
    }

    //given 4 DNA characters, return an ASCII character
    private static char DNASeqtoASCII(string DNASequence)
    {
        if (DNASequence.Length != 4) return Char.MinValue;

        int asciiValue = 0;//[0-255]
        int[] atgc = new int[4];
        for (int i = 0; i < 4; i++)
        {
            switch (DNASequence[i])
            {
                case 'A':
                    atgc[i] = 0;
                    break;
                case 'T':
                    atgc[i] = 1;
                    break;
                case 'G':
                    atgc[i] = 2;
                    break;
                case 'C':
                    atgc[i] = 3;
                    break;
            }
        }

        asciiValue += atgc[3];
        asciiValue += 4 * atgc[2];
        asciiValue += 16 * atgc[1];
        asciiValue += 64 * atgc[0];

        return (char)asciiValue;
    }

    /*(objective 3) the instructions call for an "interface"; assuming that
      is to be interpreted as "a means", rather than a literal interface.
      i.e. not "public interface IObjThree{}" */
    //Detect DNA sequences(min size:4), if any, in a given string. 
    //Returns start index or -1 if none
    public static int firstIndexOfDNA(string ascii)
    {
        int firstIndex = -1;
        int lastIndex = -1;
        for (int i = 0; i < ascii.Length; i++)
        {
            if (isDNAChar(ascii[i]))
            {
                if (firstIndex == -1) firstIndex = i;

                lastIndex = i;
            }
            else if (firstIndex != -1)//sequence ended
            {
                firstIndex = -1;
                lastIndex = -1;
            }

            if (firstIndex != -1 && lastIndex - firstIndex == 3)//end once a DNA substring was detected
            {
                return firstIndex;
            }
        }
        return -1;
    }

    //assume uppercase only
    private static bool isDNAChar(char a)
    {
        return a == 'A' || a == 'T' || a == 'G' || a == 'C';
    }


    //(objective 4) given a DNA string, return its complement, but in ASCII form.
    //assumes input string length is multiple of 4
    public static string complementDNAToASCII(string DNAstring)
    {
        string result = "";
        string complement = "";

        //complement the input.
        for (int i = 0; i < DNAstring.Length; i++)
        {
            char compChar = '?';
            switch (DNAstring[i])
            {
                case 'A':
                    compChar = 'T';
                    break;
                case 'T':
                    compChar = 'A';
                    break;
                case 'G':
                    compChar = 'C';
                    break;
                case 'C':
                    compChar = 'G';
                    break;
            }
            complement += compChar;
        }
        //parse the sequence in 4-character increments
        for (int i = 0; i < complement.Length; i += 4)
        {
            result += DNASeqtoASCII(complement.Substring(i, 4));
        }

        return result;
    }

    //(objective 5) given two DNA strands, return longest common subsequence
    public static string longestCommonSubseq(string s1, string s2)
    {
        return (s1.Length <= s2.Length) ?
            lcsHelper("", s1, s2) : lcsHelper("", s2, s1);
    }

    //shorter strand must be s1
    private static string lcsHelper(string lcs, string s1, string s2)
    {
        //basecase
        if (s1.Length == 0 || s2.Length == 0) return lcs;

        string currLCS, ss1, ss2 = "";
        char charToMatch = s1[0];
        int indexOfMatch = s2.IndexOf(charToMatch);
        ss1 = s1.Substring(1);
        ss2 = indexOfMatch > -1 ? s2.Substring(indexOfMatch + 1) : ss2;
        currLCS = indexOfMatch > -1 ? lcs + charToMatch : lcs;
        return lcsHelper(currLCS, ss1, ss2);
    }
}
