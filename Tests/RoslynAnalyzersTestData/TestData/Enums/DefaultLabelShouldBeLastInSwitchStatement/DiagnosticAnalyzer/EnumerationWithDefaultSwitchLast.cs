using System;

namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.DiagnosticAnalyzer
{
    public class EnumerationWithDefaultSwitchLast
    {
        public void EnumerationMethod(CarModels carModel)
        {
            switch (carModel)
            {
                case CarModels.Ferrari:
                    break;
                case CarModels.Lamborghini:
                    break;
                case CarModels.Mercedes:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(carModel), carModel, null);
            }
        }

        public enum CarModels
        {
            Ferrari,
            Lamborghini,
            Mercedes
        }
    }
}