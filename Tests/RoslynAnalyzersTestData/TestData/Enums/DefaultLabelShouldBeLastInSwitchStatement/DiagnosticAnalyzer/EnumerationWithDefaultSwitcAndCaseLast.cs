namespace TestData.Enums.DefaultLabelShouldBeLastInSwitchStatement.DiagnosticAnalyzer
{
    public class EnumerationWithDefaultSwitcAndCaseLast
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
