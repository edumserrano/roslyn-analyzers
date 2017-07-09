using System;

namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.CodeFixProvider
{
    public class EnumerationWithDefaultSwitcAndCasehNotLast
    {
        public void EnumerationMethod(CarModels carModel)
        {
            switch (carModel)
            {
                case CarModels.Ferrari:
                    break;
                default:
                case CarModels.Lamborghini:
                    break;
                case CarModels.Mercedes:
                    break;
            }
        }
    }
}
