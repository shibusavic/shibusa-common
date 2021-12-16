using Xunit;
using Xunit.Abstractions;

namespace Shibusa.Transformations.UnitTests
{
    public class TransformRawTextTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TransformRawTextTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CovertNewLinesAndTabsToSpaces_Null_Null()
        {
            Assert.Null(TransformRawText.ConvertNewLinesAndTabsToSpaces(null));
        }

        [Fact]
        public void CovertNewLinesAndTabsToSpaces_Empty_Empty()
        {
            Assert.Equal(string.Empty, TransformRawText.ConvertNewLinesAndTabsToSpaces(string.Empty));
        }

        [Fact]
        public void CovertNewLinesAndTabsToSpaces_NoMatch_NoChange()
        {
            const string Empty = "    ";
            const string NoNewlinesOrTabsHere = "I am string, hear me roar.";
            Assert.Equal(Empty, TransformRawText.ConvertNewLinesAndTabsToSpaces(Empty));
            Assert.Equal(NoNewlinesOrTabsHere, TransformRawText.ConvertNewLinesAndTabsToSpaces(NoNewlinesOrTabsHere));
        }

        [Fact]
        public void CovertNewLinesAndTabsToSpaces_ManyChanges_AllMade()
        {
            string testMe = $"\t{Environment.NewLine}I{Environment.NewLine}am\tstring,\t\t\thear\t{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}me{Environment.NewLine}roar.\t\t\t{Environment.NewLine}";
            string expected = "  I am string,   hear    me roar.    ";
            Assert.Equal(expected, TransformRawText.ConvertNewLinesAndTabsToSpaces(testMe));
        }

        [Fact]
        public void CondenseSpacingAndTrim_Null_Null()
        {
            Assert.Null(TransformRawText.CondenseSpacingAndTrim(null));
        }

        [Fact]
        public void CondenseSpacingAndTrim_AllWhitespace_OneSpace()
        {
            Assert.Equal(string.Empty, TransformRawText.CondenseSpacingAndTrim("         "));
        }

        [Fact]
        public void CondenseSpacingAndTrim_NoDoubleSpacing_NoChange()
        {
            string expected = "I am string, I am constant.";
            Assert.Equal(expected, TransformRawText.CondenseSpacingAndTrim(expected));
        }

        [Fact]
        public void CondenseSpacingAndTrim_DoubleSpacing_Changed()
        {
            string testMe = $"\t  I    am\tstring,\t\t\thear\t \t \tme    roar.\t\t\t";
            string expected = "I am string, hear me roar.";
            Assert.Equal(expected, TransformRawText.CondenseSpacingAndTrim(testMe));
        }

        [Fact]
        public void DataTagTransform()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello <data name=\"FirstName\" />!</p></body></html>";
            string expected = $"<html><body><p>Hello {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string> { { "FirstName", firstName } };
            var output = TransformRawText.Transform(template: text,
                keyValuePairs: dictionary,
                indexOfGroupWithKey: 1);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);

            output = TransformRawText.TransformDataTags(text, dictionary);
            Assert.Equal(expected, output);
        }

        [Fact]
        public void BraceTransform()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello {FirstName}!</p></body></html>";
            string expected = $"<html><body><p>Hello {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string> { { "FirstName", firstName } };

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: dictionary,
                regularExpression: TransformRawText.CurlyBraceExpression,
                indexOfGroupWithKey: 1);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);

            output = TransformRawText.TransformBraces(template: text,
                keyValuePairs: dictionary);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void BracketTransform()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello [FirstName]!</p></body></html>";
            string expected = $"<html><body><p>Hello {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string> { { "FirstName", firstName } };

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: dictionary,
                regularExpression: TransformRawText.BracketExpression,
                indexOfGroupWithKey: 1);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);

            output = TransformRawText.TransformBrackets(template: text,
                keyValuePairs: dictionary);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void PoundTransform()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello #FirstName#!</p></body></html>";
            string expected = $"<html><body><p>Hello {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string> { { "firstname", firstName } };
            var output = TransformRawText.Transform(template: text,
                keyValuePairs: dictionary,
                regularExpression: TransformRawText.PoundExpression,
                indexOfGroupWithKey: 1);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);

            output = TransformRawText.TransformPounds(text, dictionary);
            Assert.Equal(expected, output);
        }

        [Fact]
        public void MultilineTransform()
        {
            string firstName = "Kitty";
            string text = @"<html>
<body>
<p>
Hello #FirstName#!
</p>
</body>
</html>";
            string expected = text.Replace("#FirstName#", firstName);

            testOutputHelper.WriteLine(text);

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: new Dictionary<string, string> { { "firstname", firstName } },
                regularExpression: TransformRawText.PoundExpression,
                indexOfGroupWithKey: 1);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void RecursiveTransform()
        {
            string firstName = "<data name=\"fullname\"/>";
            string fullName = "Kitty Kat";

            string text = "<html><body><p>Hello <data name=\"FirstName\" />!</p></body></html>";
            string expected = $"<html><body><p>Hello {fullName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string>() {
                { "firstname",  firstName},
                { "fullname", fullName}
            };

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: dictionary,
                indexOfGroupWithKey: 1,
                recursive: true);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void CaseSensitive_DuplicateKeys_Throws()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello <data name=\"FirstName\" />!</p></body></html>";
            string expected = $"<html><body><p>Hello {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string>() {
                { "firstname", firstName },
                { "FIRSTNAME",firstName.ToUpper()}
            };

            Assert.Equal(2, dictionary.Count);

            Assert.Throws<ArgumentException>(() =>
            {
                var output = TransformRawText.Transform(template: text,
                    keyValuePairs: new Dictionary<string, string> { { "firstname", firstName },
                        { "FIRSTNAME",firstName.ToUpper()} },
                    indexOfGroupWithKey: 1);
            });
        }

        [Fact]
        public void MissingKeyInText_Throws()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello #lastname#, #FirstName#!</p></body></html>";

            testOutputHelper.WriteLine(text);

            Assert.Throws<Exception>(() =>
            {
                var output = TransformRawText.Transform(template: text,
                    keyValuePairs: new Dictionary<string, string> { { "firstname", firstName } },
                    regularExpression: TransformRawText.PoundExpression);
            });
        }

        [Fact]
        public void MissingKeyInText_Ignored()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello #lastname#, #FirstName#!</p></body></html>";
            string expected = $"<html><body><p>Hello #lastname#, {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: new Dictionary<string, string> { { "firstname", firstName } },
                regularExpression: TransformRawText.PoundExpression,
                indexOfGroupWithKey: 1,
                throwOnMissingKeys: false,
                replaceMissingKeysWithEmptySpace: false);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void MissingKeyInText_Removed()
        {
            string firstName = "Kitty";
            string text = "<html><body><p>Hello #lastname#, #FirstName#!</p></body></html>";
            string expected = $"<html><body><p>Hello , {firstName}!</p></body></html>";

            testOutputHelper.WriteLine(text);

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: new Dictionary<string, string> { { "firstname", firstName } },
                regularExpression: TransformRawText.PoundExpression,
                indexOfGroupWithKey: 1,
                throwOnMissingKeys: false,
                replaceMissingKeysWithEmptySpace: true);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void ComplexTest()
        {
            string firstName = "Kitty";
            string description = "Test Your Transformations";
            string game1 = "Chess";
            string game2 = "Checkers";
            string game3 = "Backgammon";
            string game4 = "Poker";
            string game5 = "Theaterwide Biotoxic and Chemical Warfare";
            string game6 = "Global Thermonuclear War";

            string text = @"<html>
<head>
    <meta name=""description"" content=""# meta-description #"">
</head>
<body>
<div>
<p>Hello # first-name#, would you like to play a game?</p>
<ol>
<li>#game-1  #
</li>
<li># game-2 #
</li>
<li>#   game-3    #
</li>
<li>#game-4#
</li>
<li>#game-5#
</li>
<li>#game-6#
</li>
</ol>
</div>
</body>
</html>";

            string expected = text.Replace("# meta-description #", description)
                .Replace("# first-name#", firstName)
                .Replace("#game-1  #", game1)
                .Replace("# game-2 #", game2)
                .Replace("#   game-3    #", game3)
                .Replace("#game-4#", game4)
                .Replace("#game-5#", game5)
                .Replace("#game-6#", game6);

            testOutputHelper.WriteLine(text);

            var dictionary = new Dictionary<string, string>() {
                { "meta-description", description},
                { "first-name", firstName},
                { "game-1", game1},
                { "game-2", game2},
                { "game-3", game3},
                { "game-4", game4},
                { "game-5", game5},
                { "game-6", game6},

            };
            var output = TransformRawText.Transform(template: text,
                keyValuePairs: dictionary,
                regularExpression: TransformRawText.PoundExpression,
                indexOfGroupWithKey: 1);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);
        }

        [Fact]
        public void No_Infinite_Loops()
        {
            string text = "<html><body><p>Hello #firstname#!</p></body></html>";
            string expected = text;

            testOutputHelper.WriteLine(text);

            var output = TransformRawText.Transform(template: text,
                keyValuePairs: new Dictionary<string, string> {
                    { "firstname", "#lastname#" },
                    { "lastname", "#firstname#" },
                },
                regularExpression: TransformRawText.PoundExpression,
                indexOfGroupWithKey: 1,
                throwOnMissingKeys: false,
                recursive: true,
                maxRecursiveIterations: 9);

            testOutputHelper.WriteLine(output);

            Assert.Equal(expected, output);
        }
    }
}
