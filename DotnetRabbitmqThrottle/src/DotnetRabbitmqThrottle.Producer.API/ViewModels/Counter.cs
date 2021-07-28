namespace DotnetRabbitmqThrottle.Producer.API.ViewModels
{
    public class Counter
    {
        private int _currentValue = 0;

        public int Value { get => _currentValue; }

        public void Inc(int? value = null)
        {
            _currentValue += value ?? 1;
        }
    }
}