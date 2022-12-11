using Xunit;

namespace Shibusa.Transformations.UnitTests
{
    public class NumberToWordsTests
    {
        private readonly string[] zeroToNineteen = { "zero",  "one",   "two",  "three", "four", "five", "six",
            "seven", "eight", "nine", "ten",   "eleven", "twelve", "thirteen",
            "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        private readonly string[] tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        [Fact]
        public void Num2Words_LowNumbers()
        {
            for (int i = 0; i < zeroToNineteen.Length; i++)
            {
                Assert.Equal(zeroToNineteen[i], NumbersToWords.ConvertToWords(i));
            }
        }

        [Fact]
        public void Num2Words_Tens()
        {
            int tensIndex = 0;
            for (int tensPos = 20; tensPos < 100; tensPos += 10)
            {
                string tensString = tens[tensIndex];
                for (int ones = 0; ones < 9; ones++)
                {
                    if (ones == 0)
                    {
                        Assert.Equal(tensString, NumbersToWords.ConvertToWords(tensPos));
                    }
                    else
                    {
                        Assert.Equal($"{tensString}-{zeroToNineteen[ones]}", NumbersToWords.ConvertToWords(tensPos + ones));
                    }
                }
                tensIndex++;
            }
        }

        [Fact]
        public void Num2Words_Hundreds()
        {
            for (int i = 1; i < 10; i++)
            {
                int tensIndex = 0;
                string hundreds = $"{zeroToNineteen[i]} hundred";
                Assert.Equal(hundreds, NumbersToWords.ConvertToWords(100 * i));

                for (int j = 1; j < zeroToNineteen.Length; j++)
                {
                    Assert.Equal($"{zeroToNineteen[i]} hundred {zeroToNineteen[j]}", NumbersToWords.ConvertToWords((i * 100) + j));
                }

                for (int tensPos = 20; tensPos < 100; tensPos += 10)
                {
                    string tensString = tens[tensIndex];
                    for (int ones = 0; ones < 9; ones++)
                    {
                        if (ones == 0)
                        {
                            Assert.Equal(tensString, NumbersToWords.ConvertToWords(tensPos));
                        }
                        else
                        {
                            Assert.Equal($"{zeroToNineteen[i]} hundred {tensString}-{zeroToNineteen[ones]}", NumbersToWords.ConvertToWords((i * 100) + tensPos + ones));
                        }
                    }
                    tensIndex++;
                }
            }
        }

        [Fact]
        public void Num2Words_Thousands()
        {
            string expected = "one thousand";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(1000));

            expected = "ten thousand one hundred ninety-nine";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(10199));

            expected = "one hundred thousand thirteen";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(100013));

            expected = "eight hundred thousand nine hundred fifty-one";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(800951));

            expected = "six hundred thirteen thousand three hundred seventy";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(613370));
        }

        [Fact]
        public void Num2Words_Millions()
        {
            string expected = "one million";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(1000000));

            expected = "four hundred million ten thousand one hundred ninety-nine";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(400010199));

            expected = "seventy-five million one hundred thousand thirteen";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(75100013));
        }

        [Fact]
        public void Num2Words_Negatives()
        {
            var minusTerm = "negative";
            string expected = $"{minusTerm} one million";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(-1000000));

            expected = $"{minusTerm} four hundred million ten thousand one hundred ninety-nine";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(-400010199));

            expected = $"{minusTerm} seventy-five million one hundred thousand thirteen";
            Assert.Equal(expected, NumbersToWords.ConvertToWords(-75100013));
        }

        [Fact]
        public void Num2Words_Extremes()
        {
            var expected = "nine quintillion two hundred twenty-three quadrillion three hundred seventy-two trillion thirty-six billion eight hundred fifty-four million seven hundred seventy-six thousand eight hundred seven";
            var words = NumbersToWords.ConvertToWords(long.MaxValue);
            Assert.Equal(expected, words);

            expected = "negative nine quintillion two hundred twenty-three quadrillion three hundred seventy-two trillion thirty-six billion eight hundred fifty-four million seven hundred seventy-six thousand eight hundred eight";
            words = NumbersToWords.ConvertToWords(long.MinValue);
            Assert.Equal(expected, words);
        }

        [Fact]
        public void Num2Words_Strings_NotANumber()
        {
            Assert.Throws<ArgumentNullException>(() => NumbersToWords.ConvertToWords(string.Empty));
            Assert.Throws<ArgumentException>(() => NumbersToWords.ConvertToWords("d"));
            Assert.Throws<ArgumentException>(() => NumbersToWords.ConvertToWords("-"));
        }

        [Fact]
        public void Num2Words_Strings_TooBig()
        {
            Assert.Throws<ArgumentException>(() => NumbersToWords.ConvertToWords("3098309830309830983030983098303098309830309830983030983098301114"));
        }

        [Fact]
        public void Num2Words_Strings()
        {
            Assert.Equal(zeroToNineteen[0], NumbersToWords.ConvertToWords("000000000000000000000000000000"));
            Assert.Equal(zeroToNineteen[0], NumbersToWords.ConvertToWords("-000000000000"));
            for (int i = 0; i < 20; i++)
            {
                string num = i.ToString();
                Assert.Equal(zeroToNineteen[i], NumbersToWords.ConvertToWords(num));
            }

            int tensIndex = 0;
            for (int tensPos = 20; tensPos < 100; tensPos += 10)
            {
                string tensString = tens[tensIndex];
                for (int ones = 0; ones < 9; ones++)
                {
                    if (ones == 0)
                    {
                        Assert.Equal(tensString, NumbersToWords.ConvertToWords(tensPos.ToString()));
                    }
                    else
                    {
                        Assert.Equal($"{tensString}-{zeroToNineteen[ones]}", NumbersToWords.ConvertToWords((tensPos + ones).ToString()));
                    }
                }
                tensIndex++;
            }

            for (int i = 1; i < 10; i++)
            {
                tensIndex = 0;
                string hundreds = $"{zeroToNineteen[i]} hundred";
                Assert.Equal(hundreds, NumbersToWords.ConvertToWords((100 * i).ToString()));

                for (int j = 1; j < zeroToNineteen.Length; j++)
                {
                    Assert.Equal($"{zeroToNineteen[i]} hundred {zeroToNineteen[j]}", NumbersToWords.ConvertToWords(((i * 100) + j).ToString()));
                }

                for (int tensPos = 20; tensPos < 100; tensPos += 10)
                {
                    string tensString = tens[tensIndex];
                    for (int ones = 0; ones < 9; ones++)
                    {
                        if (ones == 0)
                        {
                            Assert.Equal(tensString, NumbersToWords.ConvertToWords(tensPos.ToString()));
                        }
                        else
                        {
                            Assert.Equal($"{zeroToNineteen[i]} hundred {tensString}-{zeroToNineteen[ones]}", NumbersToWords.ConvertToWords(((i * 100) + tensPos + ones).ToString()));
                        }
                    }
                    tensIndex++;
                }
            }

            var expected = "one thousand";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("1000"));

            expected = "ten thousand one hundred ninety-nine";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("10199"));

            expected = "one hundred thousand thirteen";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("100013"));

            expected = "eight hundred thousand nine hundred fifty-one";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("800951"));

            expected = "six hundred thirteen thousand three hundred seventy";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("613370"));

            expected = "one million";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("1000000"));

            expected = "four hundred million ten thousand one hundred ninety-nine";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("400010199"));

            expected = "seventy-five million one hundred thousand thirteen";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("75100013"));

            expected = "negative one million";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("-1000000"));

            expected = "negative four hundred million ten thousand one hundred ninety-nine";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("-400010199"));

            expected = "negative seventy-five million one hundred thousand thirteen";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("-75100013"));

            expected = "three hundred nine vigintillion eight hundred thirty novemdecillion nine hundred eighty-three octodecillion thirty septendecillion nine hundred eighty-three sexdecillion ninety-eight quattuordecillion three hundred three tredecillion ninety-eight duodecillion three hundred nine undecillion eight hundred thirty decillion three hundred nine nonillion eight hundred thirty octillion nine hundred eighty-three septillion thirty sextillion nine hundred eighty-three quintillion ninety-eight quadrillion three hundred three trillion ninety-eight billion three hundred nine million eight hundred thirty thousand one hundred eleven";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("309830983030983098303098309830309830983030983098303098309830111"));

            expected = "negative nine hundred thirty-nine vigintillion eight hundred seventy-two novemdecillion nine hundred seventy-three octodecillion nine hundred twenty-four septendecillion one hundred six sexdecillion nine hundred twenty-nine quattuordecillion nine hundred tredecillion six hundred ninety-seven undecillion three hundred twenty-nine decillion seven hundred sixty-three nonillion nine hundred twenty-nine octillion seven hundred thirty-nine septillion seven hundred twenty-three sextillion thirty quintillion three quadrillion nine hundred seventy-two trillion nine hundred thirty billion thirty-nine million eight hundred seventy-three thousand nine hundred thirty-seven";
            Assert.Equal(expected, NumbersToWords.ConvertToWords("-939872973924106929900000697329763929739723030003972930039873937"));
        }
    }
}
