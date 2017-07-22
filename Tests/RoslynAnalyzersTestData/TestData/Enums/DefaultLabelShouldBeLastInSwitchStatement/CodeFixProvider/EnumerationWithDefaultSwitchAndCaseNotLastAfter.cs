namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.CodeFixProvider
{
    public class EnumerationWithDefaultSwitchAndCaseNotLast
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

        public enum CarModels
        {
            Ferrari,
            Lamborghini,
            Mercedes
        }
    }
}