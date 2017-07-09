using System;

namespace TestData.Enums.MergeSwitchSectionsWithEquivalentContent.CodeFixProvider
{
    public class EnumWithEquivalentSwitchSectionContent
    {
        public int EnumerationMethod(CarModels carModel)
        {
            switch (carModel)
            {
                case CarModels.Ferrari:
                    return 0;
                case CarModels.Lamborghini:
                    return 0;
                case CarModels.Mercedes:
                    return 1;
                default:
                    return 1;
            }
        }
    }
}
