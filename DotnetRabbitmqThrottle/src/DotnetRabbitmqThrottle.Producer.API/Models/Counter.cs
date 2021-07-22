namespace DotnetRabbitmqThrottle.Producer.API.Models
{
    public class Counter
    {
        private int _currentValue = 0;

        public int Value { get => _currentValue; }

        public void Inc()
        {
            _currentValue++;
        }
    }
}