using System.Collections.Generic;

namespace Test.__test
{
    public class testclass
    {
        public void testmethod()
        {
            List<voertuig> voertuigenList = new List<voertuig>();
            Auto randomAuto = new Auto() {Model = "hoi", Name = "hoi",RijAfstand = 15, Test = "testa"};
            Vliegtuig randomVliegtuig = new Vliegtuig() { Model = "hoi", Name = "hoi", Vliegafstand = "10", piloot = "pilootjeeeeh" };
            voertuigenList.Add(randomAuto);
            voertuigenList.Add(randomVliegtuig);
            Auto test = (Auto) voertuigenList[0];
            string testtxt = test.Test;
            ;
            ;

        }
    }
}