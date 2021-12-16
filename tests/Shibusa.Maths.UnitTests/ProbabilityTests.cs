using Xunit;

namespace Shibusa.Maths.UnitTests
{
    public class ProbabilityTests
    {
        [Fact]
        public void Probability_Equality()
        {
            Probability a = new(4D, 52D);
            Probability b = new(4D, 52D);
            Assert.Equal(a, b);
            b = new Probability(26D, 52D);
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Probability_Likelihood_Complement()
        {
            Probability a = new(4D, 52D);
            Assert.Equal(4D / 52D, a.Likelihood);
            Assert.Equal(1D - (4D / 52D), a.Complement);
        }

        [Fact]
        public void Probability_IndependentEvent()
        {
            Probability coinFlip = new(1D, 2D);

            // flip it twice
            Assert.Equal(.25D, Probability.IndependentProbability(new Probability[] { coinFlip, coinFlip }));
        }

        [Fact]
        public void Probability_MutuallyExclusive()
        {
            Probability chanceOfRolling3 = new(1D, 6D);
            Probability chanceOfRollingOdd = new(3D, 6D);

            Assert.Equal(2D / 3D, Probability.MutuallyExclusiveProbability(new Probability[] { chanceOfRolling3, chanceOfRollingOdd }));
        }

        [Fact]
        public void Probability_NonMutuallyExclusive()
        {
            Probability chanceOfDrawingRedCard = new(26D, 52D);
            Probability chanceOfDrawingKing = new(4D, 52D);

            // chance of drawing either red card or king
            double expected = (26D / 52D) + (4D / 52D) - (2D / 52D);
            Assert.Equal(expected, Probability.NonMutuallyExclusiveProbability(new Probability[] { chanceOfDrawingRedCard, chanceOfDrawingKing }));
        }

        /// <summary>
        /// What are the chances of drawing a King given that an ace has already been drawn.
        /// <seealso cref="https://www.thoughtco.com/conditional-probability-3126575"/>
        /// </summary>
        [Fact]
        public void Probability_Conditional1()
        {
            Probability firstDraw = new(4D, 52D);
            Probability secondDraw = new(4D, 51D);

            double expected = (16D / 2652D) / (4D / 52D);

            double liklihood = Probability.ConditionalProbability(firstDraw, secondDraw);
            Assert.Equal(expected, liklihood);
        }

        /// <summary>
        /// What is the probability that the total of two dice will be greater than 9, given that the first die is a 5?
        /// <seealso cref="https://www.onlinemathlearning.com/conditional-probability.html"/>
        /// </summary>
        [Fact]
        public void Probability_Conditional2()
        {
            Probability firstDie = new(1D, 6D);
            Probability secondDie = new(2D, 6D); // given a 5, the second die can only be a 5 or a 6.

            double expected = 1D / 3D;

            Assert.Equal(expected, Probability.ConditionalProbability(firstDie, secondDie));
        }
    }
}
