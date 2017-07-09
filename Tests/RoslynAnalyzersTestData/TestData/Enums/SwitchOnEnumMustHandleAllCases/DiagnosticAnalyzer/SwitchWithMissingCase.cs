using System;

namespace TestData.Enums.SwitchOnEnumMustHandleAllCases.DiagnosticAnalyzer
{
    public class SwitchWithMissingCase
    {
        public void EnumerationMethod(CarModels carModel)
        {
            switch (carModel)
            {
                case CarModels.Ferrari:
                    break;
                case CarModels.Lamborghini:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(carModel), carModel, null);
            }
        }
    }
}
