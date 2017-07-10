namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.DiagnosticAnalyzer
{
    public class EnumerationWithDefaultSwitchAndCasehNotLast
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
