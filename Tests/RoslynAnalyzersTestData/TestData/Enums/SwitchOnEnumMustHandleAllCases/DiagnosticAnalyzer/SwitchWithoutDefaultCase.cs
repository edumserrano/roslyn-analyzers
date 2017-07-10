namespace TestData.Enums.SwitchOnEnumMustHandleAllCases.DiagnosticAnalyzer
{
    public class SwitchWithoutDefaultCase
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
