using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace MyAdventTests

{
    [TestClass]
    public class AdventOfCode17_24
    {
        [TestInitialize]
        public void Setup()
        {

        }

        private long RegisterA = 0;
        private long RegisterB = 0;
        private long RegisterC = 0;

        [TestMethod]
        public void Day17A()
        {
            (int, int, int, List<int>) data = FileReader.LoadRegisters("17A.txt");
            RegisterA = data.Item1;
            RegisterB = data.Item2;
            RegisterC = data.Item3;
            List<int> program = data.Item4;
            string output = "";
            int instructionPointer = 0;
            bool jump = false;
            while (instructionPointer < program.Count)
            {
                int task = program[instructionPointer];
                int literaloperand = program[instructionPointer + 1];
                long combooperand = GetComboOperand(program[instructionPointer + 1]);
                switch (task)
                {

                    case 0:
                        {
                            RegisterA = RegisterA / (int)Math.Pow(2, combooperand);
                            break;
                        }
                    case 1:
                        {
                            RegisterB = XORBitwise(RegisterB, literaloperand);
                            break;
                        }
                    case 2:
                        {
                            RegisterB = combooperand % 8;
                            break;
                        }
                    case 3:
                        {
                            if (RegisterA == 0)
                            {
                                break;
                            }
                            instructionPointer = literaloperand;
                            jump = true;
                            break;
                        }
                    case 4:
                        {
                            RegisterB = XORBitwise(RegisterB, RegisterC);
                            break;
                        }
                    case 5:
                        {
                            if (output.Length > 0)
                                output += ",";
                            output += combooperand % 8;
                            break;
                        }

                    case 6:
                        {
                            RegisterB = RegisterA / (int)Math.Pow(2, combooperand);
                            break;
                        }
                    case 7:
                        {
                            RegisterC = RegisterA / (int)Math.Pow(2, combooperand);
                            break;
                        }
                }
                if (!jump)
                    instructionPointer += 2;
                else
                    jump = false;
            }
            Assert.AreEqual("2,1,0,1,7,2,5,0,3", output); // 4,6,3,5,6,3,5,2,1,0
        }

        [TestMethod]
        public void Day17B()
        {
            (int, int, int, List<int>) data = FileReader.LoadRegisters("17A.txt");
            RegisterA = data.Item1;

            List<int> program = data.Item4;
            string output = "";
            int instructionPointer = 0;
            bool jump = false;
            long counter = (long)33408145777732 * 8-100000;
            long counter2 = 0;
                int outputlength = 0;
            while (output != "2,4,1,7,7,5,0,3,4,4,1,7,5,5,3,0") //"2,4,1,7,7,5,0,3,4, 4, 1, 7,5,5,3,0"
            { 
                if (counter2==200000)
                {
                    outputlength = output.Length;
                }
                counter++;
                counter2++;
                jump = false;
                instructionPointer = 0;
                RegisterA = counter;
                RegisterB = data.Item2;
                RegisterC = data.Item3;
                output = "";
                while (instructionPointer < program.Count)
                {
                    int task = program[instructionPointer];
                    int literaloperand = program[instructionPointer + 1];
                    long combooperand =  GetComboOperand(program[instructionPointer + 1]);
                    switch (task)
                    {

                        case 0:
                            {
                                RegisterA = RegisterA / (int)Math.Pow(2, combooperand);
                                break;
                            }
                        case 1:
                            {
                                RegisterB = XORBitwise(RegisterB, literaloperand);
                                break;
                            }
                        case 2:
                            {
                                RegisterB = combooperand % 8;
                                break;
                            }
                        case 3:
                            {
                                if (RegisterA == 0)
                                {
                                    break;
                                }
                                instructionPointer = literaloperand;
                                jump = true;
                                break;
                            }
                        case 4:
                            {
                                RegisterB = XORBitwise(RegisterB, RegisterC);
                                break;
                            }
                        case 5:
                            {
                                if (output.Length > 0)
                                    output += ",";
                                output += combooperand % 8;
                                break;
                            }

                        case 6:
                            {
                                RegisterB = RegisterA / (int)Math.Pow(2, combooperand);
                                break;
                            }
                        case 7:
                            {
                                RegisterC = RegisterA / (int)Math.Pow(2, combooperand);
                                break;
                            }
                    }
                    if (!jump)
                        instructionPointer += 2;
                    else
                        jump = false;
                }
            }
            Assert.AreEqual("2,4,1,7,7,5,0,3,4,4,1,7,5,5,3,0", output); // 4,6,3,5,6,3,5,2,1,0
        }

        private int TowelCounter= 0;

        [TestMethod]
        public void Day19A()
        {
            (List<string>, List<string>) data = FileReader.LoadTowlPattern("19A.txt");
            List<string> pattern = data.Item1;
            List<string> designs = data.Item2;
            int counter= 0;
            foreach (var design in designs)
            {
                if (SearchWithPatternCache(pattern, design)>0)
                {
                    counter++;
                }
            }

            Assert.AreEqual(216, counter);
        }

        [TestMethod]
        public void Day19B()
        {
            (List<string>, List<string>) data = FileReader.LoadTowlPattern("19A.txt");
            List<string> pattern = data.Item1;
            List<string> designs = data.Item2;
            TowelCounter = 0;
            long counter = 0;
            foreach (var design in designs)
            {
                counter += (SearchWithPatternCache(pattern, design));              
            }


            Assert.AreEqual(603191454138773, counter);
        }

        Dictionary<string, long> Cache = new Dictionary<string, long>();

        private long SearchWithPatternCache(List<string> pattern, string design)
        {
            if (Cache.TryGetValue(design, out long value))
            {
                return value;
            }
            List<string> foundPattern = FindAllPattern(pattern, design);
            if (foundPattern.Count == 0)
            {  return 0; }

            long patterncounter = 0;
            foreach (var thispattern in foundPattern)
            {
                var newdesign = design.Substring(thispattern.Length);
                if (newdesign.Length == 0)
                {
                    patterncounter++;
                    continue;
                }

                patterncounter += SearchWithPatternCache(pattern, newdesign);
            }

            if (Cache.ContainsKey(design))
            {
                Cache[design] = patterncounter;
            } else
            {
                Cache.Add(design, patterncounter);
            }
            return patterncounter;
        }

        private List<string> FindAllPattern(List<string> pattern, string design)
        {
            List<string> foundPattern = new List<string>();
            foreach (var pat in pattern)
            {
                if (design.StartsWith(pat))
                {
                    foundPattern.Add(pat);
                }
            }
            return foundPattern;
        }

        private long XORBitwise(long num1, long num2)
        {

            var number1 = Convert.ToString(num1, 2);
            var number2 = Convert.ToString(num2, 2);
            var xornumber = "";
            int length = Math.Max(number1.Length, number2.Length);
            while (length > 0)
            {
                var bit1 = number1.Last();
                var bit2 = number2.Last();
                if (bit1 == bit2)
                    xornumber = "0" + xornumber;
                else
                    xornumber = "1" + xornumber;

                number1=number1.Remove(number1.Length - 1, 1);
                number2 =number2.Remove(number2.Length - 1, 1);
                length--;
                if (number1.Length == 0)
                {
                    xornumber = number2 + xornumber;
                    break;
                }
                if (number2.Length == 0)

                { xornumber = number1 + xornumber;
                    break;
                }
            }
            return Convert.ToInt64(xornumber, 2);
        }


        private long GetComboOperand(int v)
        {
            if (v <= 3)
                return v;
            if (v == 4)
                return RegisterA;
            if (v == 5)
                return RegisterB;
            if (v == 6)
                return RegisterC;
            return 7;
        }
    }
}