namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.DiagnosticAnalyzer
{
    public class EnumerationWithDefaultSwitchAndCaseLast
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
