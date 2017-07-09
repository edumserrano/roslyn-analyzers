using System;

namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.CodeFixProvider
{
    public class EnumerationWithDefaultSwitchNotLast
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