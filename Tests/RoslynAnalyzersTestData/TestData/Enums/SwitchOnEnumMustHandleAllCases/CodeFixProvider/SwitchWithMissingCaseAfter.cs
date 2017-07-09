using System;

namespace TestData.Enums.SwitchOnEnumMustHandleAllCases.CodeFixProvider
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
                case CarModels.Mercedes:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(carModel), carModel, null);
            }
        }
    }
}