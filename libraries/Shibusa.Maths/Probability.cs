namespace Shibusa.Maths
{
    /// <summary>
    /// Represents the probability of an event.
    /// </summary>
    public class Probability
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Probability"/> class.
        /// </summary>
        /// <param name="numberOfWaysEventCanHappen">The number of ways an event can happen.</param>
        /// <param name="numberOfAllPossibleOutcomes">The number of all possible outcomes.</param>
        public Probability(double numberOfWaysEventCanHappen,
            double numberOfAllPossibleOutcomes)
        {
            NumberOfWaysEventCanHappen = numberOfWaysEventCanHappen;
            NumberOfAllPossibleOutcomes = numberOfAllPossibleOutcomes;
        }

        /// <summary>
        /// Gets the number of ways an event can happen.
        /// </summary>
        public double NumberOfWaysEventCanHappen { get; }

        /// <summary>
        /// Gets the number of all possible outcomes.
        /// </summary>
        public double NumberOfAllPossibleOutcomes { get; }

        /// <summary>
        /// Gets the probability this event will occur.
        /// </summary>
        public double Likelihood => NumberOfWaysEventCanHappen / NumberOfAllPossibleOutcomes;

        /// <summary>
        /// Gets the complement of <see cref="Likelihood"/> (1 - <see cref="Likelihood"/>).
        /// </summary>
        public double Complement => 1 - Likelihood;

        /// <summary>
        /// Determine the likelihood of a collection of events occurring.
        /// </summary>
        /// <param name="events">The collection of events to evaluate.</param>
        /// <returns>The likliehood of all events occurring.</returns>
        public static double IndependentProbability(Probability[] events)
        {
            double result = 0D;

            if (events?.Any() ?? false)
            {
                result = 1D;

                foreach (Probability p in events)
                {
                    result *= p.Likelihood;
                }
            }

            return result;
        }

        /// <summary>
        /// Determine the liklihood of one of the events occurring.
        /// </summary>
        /// <param name="events">The collection of events to evaluate.</param>
        /// <returns>The liklihood of any event occurring.</returns>
        public static double MutuallyExclusiveProbability(Probability[] events)
        {
            double result = 0D;

            if (events?.Any() ?? false)
            {
                result = 0D;

                foreach (Probability p in events)
                {
                    result += p.Likelihood;
                }
            }

            return result;
        }

        /// <summary>
        /// Determine the likelihood of either one or all events occurring.
        /// </summary>
        /// <param name="events">The collection of events to evaluate.</param>
        /// <returns>The liklihood of either one or all events occurring.</returns>
        public static double NonMutuallyExclusiveProbability(Probability[] events)
        {
            return MutuallyExclusiveProbability(events) - IndependentProbability(events);
        }

        /// <summary>
        /// Determine the likelihood of an event occurring given that another event has already occurred.
        /// </summary>
        /// <param name="firstEvent">The first event.</param>
        /// <param name="secondEvent">The second event.</param>
        /// <returns>The liklihood of the second event occurring given that the first event has already occurred.</returns>
        public static double ConditionalProbability(Probability firstEvent, Probability secondEvent)
        {
            return IndependentProbability(new Probability[] { firstEvent, secondEvent }) / firstEvent.Likelihood;
        }
    }
}
