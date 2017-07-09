namespace TestData.Enums.MergeSwitchSectionsWithEquivalentContent.DiagnosticAnalyzer
{
    public class EnumWithoutEquivalentSwitchSectionContent
    {
        public int EnumerationMethod(CarModels carModel)
        {
            switch (carModel)
            {
                case CarModels.Ferrari:
                    return 0;
                case CarModels.Lamborghini:
                    return 1;
                case CarModels.Mercedes:
                    return 2;
                default:
                    return 3;
            }
        }
    }
}