namespace TestData.Enums.MergeSwitchSectionsWithEquivalentContent.CodeFixProvider
{
    public class EnumWithEquivalentSwitchSectionContent
    {
        public int EnumerationMethod(CarModels carModel)
        {
            switch (carModel)
            {
                case CarModels.Ferrari:
                case CarModels.Lamborghini:
                    return 0;
                case CarModels.Mercedes:
                default:
                    return 1;
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