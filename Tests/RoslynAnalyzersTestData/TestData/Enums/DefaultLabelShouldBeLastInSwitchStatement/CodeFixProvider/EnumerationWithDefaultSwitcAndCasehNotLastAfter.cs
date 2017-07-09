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
                case CarModels.Mercedes:
                    break;
                case CarModels.Lamborghini:
                default:
                    break;
            }
        }
    }
}