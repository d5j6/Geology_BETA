using HoloToolkit.Unity;

public enum InformationsMockup { Earth, Crust, UpperMantle, UpperMantleLess, LowerMantle, LowerMantleLess, OuterCore, InnerCore, Sandstone, Granit, Basalt, Mantle, OceanicCrust, ContinentalCrust,
MagneticField, Mass, Temperature, Pie}

public class DataController : Singleton<DataController> {

    public float GetMaxScrollPosFor(InformationsMockup query)
    {
        switch (LanguageManager.Instance.CurrentLanguage)
        {
            case Language.Russian:
                switch (query)
                {
                    case InformationsMockup.Earth:
                        return 0.1f;
                    case InformationsMockup.Crust:
                        return 0.07f;
                    case InformationsMockup.LowerMantle:
                    case InformationsMockup.UpperMantle:
                    case InformationsMockup.LowerMantleLess:
                    case InformationsMockup.UpperMantleLess:
                    case InformationsMockup.Mantle:
                        if (SceneStateMachine.Instance.IsEarthContainingState())
                        {
                            return 0.11f;
                        }
                        else
                        {
                            return 0.92f;
                        }
                    case InformationsMockup.OuterCore:
                    case InformationsMockup.InnerCore:
                        return 0.088f;
                    case InformationsMockup.Granit:
                        return 0.1f;
                    case InformationsMockup.Sandstone:
                        return 0.15f;
                    case InformationsMockup.Basalt:
                        return 0.34f;
                    case InformationsMockup.Pie:
                        return 0.75f;
                }
                break;
            case Language.English:
                switch (query)
                {
                    case InformationsMockup.Earth:
                        return 0.16f;
                    case InformationsMockup.Crust:
                        return 0.12f;
                    case InformationsMockup.LowerMantle:
                    case InformationsMockup.UpperMantle:
                    case InformationsMockup.LowerMantleLess:
                    case InformationsMockup.UpperMantleLess:
                    case InformationsMockup.Mantle:
                        if (SceneStateMachine.Instance.IsEarthContainingState())
                        {
                            return 0.11f;
                        }
                        else
                        {
                            return 0.92f;
                        }
                    case InformationsMockup.OuterCore:
                    case InformationsMockup.InnerCore:
                        return 0.045f;
                    case InformationsMockup.Sandstone:
                        return 0.11f;
                    case InformationsMockup.Basalt:
                        return 0.2f;
                    case InformationsMockup.Pie:
                        return 0.65f;
                }
                break;
        }
        return 0f;
    }

    public string GetLabelFor(InformationsMockup query)
    {
        switch (LanguageManager.Instance.CurrentLanguage)
        {
            //case Language.Russian:
            case Language.English:
                switch (query)
                {
                    case InformationsMockup.Earth:
                        return "General info";
                    case InformationsMockup.Crust:
                        return "The crust";
                    case InformationsMockup.UpperMantle:
                        //return "Upper mantle";
                    case InformationsMockup.LowerMantle:
                        //return "Lower mantle";
                    case InformationsMockup.UpperMantleLess:
                        //return "Upper mantle";
                    case InformationsMockup.LowerMantleLess:
                        return "The mantle";
                    case InformationsMockup.OuterCore:
                        //return "Outer core";
                    case InformationsMockup.InnerCore:
                        return "Core";
                    case InformationsMockup.Sandstone:
                        return "Sedimentary layer";
                    case InformationsMockup.Granit:
                        return "Granite layer";
                    case InformationsMockup.Basalt:
                        return "Basalt layer";
                    case InformationsMockup.Mantle:
                        return "Mantle";
                    case InformationsMockup.OceanicCrust:
                        return "Oceanic crust";
                    case InformationsMockup.ContinentalCrust:
                        return "Continental crust";
                    case InformationsMockup.MagneticField:
                        return "The magnetic field";
                    case InformationsMockup.Mass:
                        return "Earth Mass";
                    case InformationsMockup.Temperature:
                        return "Temperature";
                    case InformationsMockup.Pie:
                        return "The crust model";
                    default:
                        return "Error";
                }
            case Language.Russian:
                switch (query)
                {
                    case InformationsMockup.Earth:
                        return "Общая информация";
                    case InformationsMockup.Crust:
                        return "Земная кора";
                    case InformationsMockup.UpperMantle:
                        return "Мантия";
                    case InformationsMockup.LowerMantle:
                        return "Мантия";
                    case InformationsMockup.UpperMantleLess:
                        return "Мантия";
                    case InformationsMockup.LowerMantleLess:
                        return "Мантия";
                    case InformationsMockup.OuterCore:
                        return "Ядро";
                    case InformationsMockup.InnerCore:
                        return "Ядро";
                    case InformationsMockup.Sandstone:
                        return "Осадочный слой";
                    case InformationsMockup.Granit:
                        return "Гранитный слой";
                    case InformationsMockup.Basalt:
                        return "Базальтовый слой";
                    case InformationsMockup.Mantle:
                        return "Мантия";
                    case InformationsMockup.OceanicCrust:
                        return "Океаническая кора";
                    case InformationsMockup.ContinentalCrust:
                        return "Континентальная кора";
                    case InformationsMockup.MagneticField:
                        return "Магнитное поле";
                    case InformationsMockup.Mass:
                        return "Масса";
                    case InformationsMockup.Temperature:
                        return "Температура";
                    default:
                        return "Ошибка";
                }
        }
        return "";
    }

    public string GetDescriptionFor(InformationsMockup query)
    {
        switch (LanguageManager.Instance.CurrentLanguage)
        {
            //case Language.Russian:
            case Language.English:
                switch (query)
                {
                    case InformationsMockup.Earth:
                        //return "<align=justified>Earth is the planet we live on.  It is the third planet from the Sun. It is the only planet known to have life on it.  The Earth formed around 4.5 billion years ago. It is one of four rocky planets on the inside of the Solar System.  The other three are Mercury, Venus and Mars.  The large mass of the Sun makes the Earth move around it, just as the mass of the Earth makes the Moon move around it.  The Earth also turns round in space, so different parts face the Sun at different times.  The Earth goes around the Sun once (one \"year\") for every 365 times it turns all the way around (one \"day\").  The Earth is the only planet in our Solar System that has a large amount of liquid water.  About 71% of the surface of the Earth is covered by oceans.  Because of this, it is sometimes called the \"Blue Planet\".</align>";
                        return @"<align=justified>  Earth is the third planet from the sun. It is the only planet known to have an atmosphere containing free oxygen, oceans of liquid water on its surface, and life. Roughly 71 percent of Earth's surface is covered by water, most of it in the oceans. About a fifth of Earth's atmosphere is made up of oxygen, produced by plants.  Earth is the fifth largest of the planets in the solar system — smaller than the four gas giants, Jupiter, Saturn, Uranus and Neptune, but larger than the three other rocky planets, Mercury, Mars and Venus.
    Earth has a radius of roughly 6371 kilometers, and is round because gravity pulls matter into a ball.
    Scientists think Earth was formed at roughly the same time as the sun and other planets some 4. 54 billion years ago. It can be divided into three general layers by their chemical or physical properties: crust, mantle and core. Mean chemical composition is represented mostly by iron (32.1%), oxygen (30.1%), silicon (15.1%), magnesium (13.9%), sulfur (2.9%), nickel (1.8%), calcium (1.5%), and aluminum (1.4%), with the remaining 1.2% consisting of trace amounts of other elements. </align>

<align=left>    <b>General characteristics: </b>
<b>Mean radius of Earth</b> – 6371 kilometers.
<b>Mass</b> - 5.97237x10<sup>24</sup> kg
<b>Volume</b> - 1.08321x1012 km<sup>3</sup>
<b>Mean density of Earth</b> – 5,52 g/cm<sup>3</sup></align>";
                    case InformationsMockup.Crust:
                        //return "<align=justified>The crust is the outer layer of our planet and ranges from 5  to 60 km in depth. It is  the solid rock layer consists of  continental crust which is thick and carries land (about 30-60 km in depth) and oceanic crust  which is thin and underlies the continents (about 3-10km in depth).  The oceanic crust is composed of basalt while the continental crust is composed of granite. Both of them are covered with sedimentary rocks. The basic composition of the crust is is represented by oxides of silicon, aluminium, iron, and other metals.   The surface of the crust is quite well studied. One of the deepest drilling wells is The Kola Superdeep Borehole, which reached a depth of 12.289 km </align>";
                        return @"<align=justified>  The crust is the outer layer of our planet and ranges from 5  to 60 kilometers in depth. It is  the solid rock layer consisting of  continental crust and oceanic crust. The continental crust is located under the continents and is from 30 to 60 kilometers thick. It is mostly composed of  sedimentary, granitic and basaltic rocks. The oceanic crust extends to a depth of  3-10 kilometers under the ocean floor and forms at spreading centers on oceanic ridges. It is mostly composed of basalt and sedimentary rocks. Both the continental and oceanic crusts float on the upper mantle. The basic composition of the crust is represented by oxides of silicon, aluminum, iron, and other metals. The surface of the crust is quite well studied. One of the deepest drilling wells is The Kola Superdeep Borehole, which reached a depth of  12.289 kilometers. </align>

<align=left>    <b>Chemical composition, %:</b>
SiО – 56.8
Al<sub>2</sub>O<sub>3</sub> – 14.8
FeO – 6.1
CaO – 8
Na<sub>2</sub>O – 2.7
K2O – 1.5
MgO – 5.1</align>";
                    case InformationsMockup.Mantle:
                    case InformationsMockup.UpperMantle:
                        //return "<align=justified>The mantle is the second layer of the Earth that takes more than two thirds of the total weight of the Earth and it is divided into upper and lower mantle.. The upper part of the mantle, which is in direct contact with the Earths crust, is called the lithosphere. It is in the solid state and extends to a depth of about 100 km. The next  300 km is made of plastic, viscous rocks  (close to the melting point).  This part of mantle is called the asthenosphere, which is the source of molten magma. Magma, penetrating through faults in the crust to the surface, forms volcanoes. The mantle composition is determined on the basis of geophysical and experimental data, which correspond to ultrabasic rocks enriched with Mg and Fe. The lower mantle.  The lower mantle extends from about 660 km to about 2400 km beneath Earths surface. It is  hotter and denser than the upper mantle and is in solid state due to intense pressure.   The average temperature of the mantle is 3000<sup>o</sup> Celsius.</align>";
                    case InformationsMockup.LowerMantle:
                        //return "<align=justified>The mantle is the second layer of the Earth that takes more than two thirds of the total weight of the Earth and it is divided into upper and lower mantle.. The upper part of the mantle, which is in direct contact with the Earths crust, is called the lithosphere. It is in the solid state and extends to a depth of about 100 km. The next  300 km is made of plastic, viscous rocks  (close to the melting point).  This part of mantle is called the asthenosphere, which is the source of molten magma. Magma, penetrating through faults in the crust to the surface, forms volcanoes. The mantle composition is determined on the basis of geophysical and experimental data, which correspond to ultrabasic rocks enriched with Mg and Fe. The lower mantle.  The lower mantle extends from about 660 km to about 2400 km beneath Earths surface. It is  hotter and denser than the upper mantle and is in solid state due to intense pressure.   The average temperature of the mantle is 3000<sup>o</sup> Celsius.</align>";
                    case InformationsMockup.UpperMantleLess:
                    case InformationsMockup.LowerMantleLess:
                        //return "<align=justified>The mantle is the second layer of the Earth that takes more than two thirds of the total weight of the Earth and it is divided into upper and lower mantle.. The upper part of the mantle, which is in direct contact with the Earths crust, is called the lithosphere.</align>";
                        return @"<align=justified>  The mantle extends to a depth of 2900 kilometers and is the second layer of the The mantle composition is determined on the basis of geophysical and experimental data, which correspond to ultrabasic rocks enriched with Mg and Fe. Earth that takes more than two thirds of the total weight of the Earth and it is divided into upper and lower mantle. The upper part of the mantle, which is in direct contact with the Earth's crust, is called the lithosphere. It is in the solid state and extends to a depth of about 100 kilometers. The next 300 kilometers is made of plastic, viscous rocks (close to the melting point).  This part of mantle is called the asthenosphere, which is the source of molten magma. Magma, penetrating through faults in the crust to the surface, forms volcanoes. The lower mantle extends from about 660 kilometers to about 2900 kilometers beneath Earth’s surface. It is hotter and denser than the upper mantle and is in solid state due to intense pressure. The average temperature of the mantle is 3000<sup>o</sup> C. </align>

<align=left>    <b>Chemical composition, %:</b>
SiО – 45.3
MgO – 41.3
FeO – 7.3
Al<sub>2</sub>O<sub>3</sub> – 3.6
CaO – 1.9</align>";
                    case InformationsMockup.OuterCore:
                        //return "<align=justified>Earths Core is extends to a depth of around 2900 km  composed mainly of an iron and nickel alloy. The core is divided into two different zones. The outer core is a liquid because the temperatures there are adequate to melt the iron-nickel alloy. However, the inner core is a solid even though its temperature is higher than the outer core. Here, tremendous pressure, produced by the weight of the overlying rocks is strong enough to crowd the atoms tightly together and prevents the liquid state.  The core is earths source of internal heat because it contains radioactive materials which release heat as they break down into more stable substances.</align>";
                    case InformationsMockup.InnerCore:
                        //return "<align=justified>Earths Core is extends to a depth of around 2900 km  composed mainly of an iron and nickel alloy. The core is divided into two different zones. The outer core is a liquid because the temperatures there are adequate to melt the iron-nickel alloy. However, the inner core is a solid even though its temperature is higher than the outer core. Here, tremendous pressure, produced by the weight of the overlying rocks is strong enough to crowd the atoms tightly together and prevents the liquid state.  The core is earths source of internal heat because it contains radioactive materials which release heat as they break down into more stable substances.</align>";
                        return @"<align=justified>  The Earth structure can be divided into three general layers by their chemical or physical properties: crust, mantle and core. Earth's Core with a radius of about 3470  kilometers  is composed mainly of an iron and nickel alloy. The core is divided into two different zones. The outer core is in liquid state because the temperatures are adequate to melt the iron-nickel alloy. However, the inner core is a solid even though its temperature is higher than the outer core. Here, tremendous pressure, produced by the weight of the overlying rocks is strong enough to crowd the atoms tightly together and prevents the liquid state. The core is earth's source of internal heat because it contains radioactive materials which release heat as they break down into more stable substances. </align>

<align=left>    <b>Chemical composition, %:</b>
Fe - 80
Ni – 9
Si - 7
O – 4
S – 2</align>";
                    case InformationsMockup.Sandstone:
                        //return "The mean thickness of sedimentary rocks ranges from 0 to 15 km. The most widespread sedimentary rocks are sand, clay, mud and limy mud. Limestones were once carbonate mud on the seafloor. This mud is made of tiny carbonate shells. Sandstone and claystone is a lithified sand and clays corresponding. Mean chemical composition is represented mostly by oxides of silicon (44%), calcium (16%), aluminium (11%) and magnesium (3%).";
                        //return "The mean thickness of sedimentary rocks ranges from 0 to 15 km. The most widespread sedimentary rocks are sand, clay, mud and limy mud. Limestones were once carbonate mud on the seafloor. This mud is made of tiny carbonate shells. Sandstone and claystone is a lithified sand and clays corresponding. Mean chemical composition is represented mostly by oxides of silicon (44%), calcium (16%), aluminium (11%) and magnesium (3%).";
                        return @"<align=justified>  The mean thickness of sedimentary rocks ranges from 0 to 15 km.The most widespread sedimentary rocks are sand, clay, mud and limy mud. Limestones were once carbonate mud on the seafloor. This mud is made of tiny carbonate shells. Sandstone and claystone is a lithified sand and clays corresponding.Mean chemical composition is represented mostly by oxides of silicon(44 %), calcium(16 %), aluminum(11 %) and magnesium (3 %).</align>";
                    case InformationsMockup.Granit:
                        //return "Granite layer is composed mostly of granites, gneisses, schists and rhyolite (volcanic equivalent of granite). The mean thickness ranges from 10 to 40 km (17 on the average). Composition of the main oxides differ from sedimentary rocks mostly by increasing of  silica (63%) and aluminium  (15%) and decreasing of calcium (4%).";
                        return @"<align=justified>  Granite layer is composed mostly of granites, gneisses, schists and rhyolite (volcanic equivalent of granite). The mean thickness ranges from 10 to 40 km (17 on the average). Composition of the main oxides differ from sedimentary rocks mostly by increasing of silica (63%) and aluminum  (15%) and decreasing of calcium (4%).</align>";
                    case InformationsMockup.Basalt:
                        //return "The mean thickness of basalt layer is about 22 km. Basalt layer of continental crust has very diverse composition with a predominance of metamorphic basic rocks such as granulites, eclogites. Basalt layer of the oceanic crust is much more homogeneous. The predominant rock type is so-called tholeiitic basalt which differs  from continental basalts by low content of potassium, rubidium, strontium, barium and uranium. The main oxides of the layer is silica (52%), aluminium (15%), calcium (9%), magnesium (7%) and iron (6%).";
                        //return "The mean thickness of basalt layer is about 22 km. Basalt layer of continental crust has very diverse composition with a predominance of metamorphic basic rocks such as granulites, eclogites. Basalt layer of the oceanic crust is much more homogeneous.";
                        return @"<align=justified>  The mean thickness of basalt layer is about 22 km. Basalt layer of continental crust has very diverse composition with a predominance of metamorphic basic rocks such as granulites, eclogites. Basalt layer of the oceanic crust is much more homogeneous. The predominant rock type is so-called tholeiitic basalt which differs from continental basalts by low content of potassium, rubidium, strontium, barium and uranium. The main oxides of the layer is silica (52%), aluminum (15%), calcium (9%), magnesium (7%) and iron (6%).</align>";
                    case InformationsMockup.OceanicCrust:
                        //return "The oceanic crust extends to a depth of  3-10 kilometers under the ocean floor and forms at spreading centres on oceanic ridges. As opposed to continental crust, oceanic crust hasn’t granite layer and is composed only of basalt and sedimentary rocks.";
                        return @"<align=justified>  The <b>oceanic crust</b> extends to a depth of 3-10 kilometers under the ocean floor and forms at spreading centers on oceanic ridges. As opposed to continental crust, oceanic crust hasn’t granite layer and is composed only of basalt and sedimentary rocks.</align>";
                    case InformationsMockup.ContinentalCrust:
                        //return "The continental crust is located under the continents and is from 30 to 60 kilometers thick. It is mostly composed of  sedimentary, granitic and basaltic rocks. The mean thickness of sedimentary layer is 5,0 km, granit layer – 17 km , basalt layer - 22 km.";
                        return @"<align=justified>  The <b>continental crust is</b> located under the continents and is from 30 to 60 kilometers thick. It is mostly composed of sedimentary, granitic and basaltic rocks. The mean thickness of sedimentary layer is 5.0 km, granit layer – 17 km, basalt layer - 22 km. </align>";
                    case InformationsMockup.MagneticField:
                        //return "The magnetic field of the Earth is generated by amperages of a liquid metal core. It is a major barrier, protecting it from the solar wind and radiation, by creating the magnetosphere around the Earth. In a case of the interaction between the magnetic field and the solar wind, magnetosphere becomes quite complex shape, streamlined by the solar wind.The magnetic poles are in constant movement, with the result that they are periodically reversed. The last time the poles switch places occurred about 700 000 years ago, and over the past 10 million years, the polarity changed 18 times!";
                        return @"<align=justified>  The magnetic field of the Earth is generated by amperages of a liquid metal core. It is a major barrier, protecting it from the solar wind and radiation, by creating the magnetosphere around the Earth. In a case of the interaction between the magnetic field and the solar wind, magnetosphere becomes quite complex shape, streamlined by the solar wind. The magnetic poles are in constant movement, with the result that they are periodically reversed. The last time the poles switch places occurred about 700 000 years ago, and over the past 10 million years, the polarity changed 18 times! </align>";
                    case InformationsMockup.Temperature:
                        //return "The average temperature of the Earth is about 15 <sup>o</sup>С . Since a beginning of the twentieth century the average temperature has increased to 5%. About two-thirds of this increas occurred after 80-th years of the last century. If present trend will continue, global warming can lead to catastrophic consequences by the end of this century. The main cause of global warming is active industrialization of mankind. The highest temperature, recorded in 1913 year in California, was 56,7 <sup>o</sup>С.The minimum temperature, recorded at the Vostok station in 1983 year, was - 89,2 <sup>o</sup>С.";
                        return @"<align=justified>  The average temperature of the Earth is about 15 <sup>º</sup>С . Since a beginning of the twentieth century the average temperature has increased to 5%. About two-thirds of this increase occurred after 80-th years of the last century. If present trend will continue, global warming can lead to catastrophic consequences by the end of this century. The main cause of global warming is active industrialization of mankind.
    The highest temperature, recorded in 1913 year in California, was 56.7 <sup>º</sup>С. The minimum temperature, recorded at the Vostok station in 1983 year, was -89.2 <sup>º</sup>С. </align>";
                    case InformationsMockup.Mass:
                        //return "The Earth mass is nearly 5.97237x10<sup>24</sup> . For comparison, the Moon's mass is about 81 times less than Earth mass. This is while the Jupiter planet more than 300 times of the Earth mass. Mass of the Earth is not constant, and is constantly changing. Currently, the weight loss exceeds gain. The main reason for growth of the Earth's mass (~ 40 000 t / y) is cosmic dust and meteors. The main factor reducing the mass of the earth (~ 96 600 t / y) is the loss of atmosphere gas (mostly hydrogen and helium) of the planet due to their dispersion into space.";
                        return @"<align=justified>  The Earth mass is nearly 5.97237x10<sup>24</sup> . For comparison, the Moon's mass is about 81 times less than Earth mass. This is while the Jupiter planet more than 300 times of the Earth mass.
    Mass of the Earth is not constant, and is constantly changing. Currently, the weight loss exceeds gain. The main reason for growth of the Earth's mass (~ 40 000 t / y) is cosmic dust and meteors. The main factor reducing the mass of the earth (~ 96 600 t / y) is the loss of atmosphere gas (mostly hydrogen and helium) of the planet due to their dispersion into space. </align>";
                    case InformationsMockup.Pie:
                        return @"<align=justified>  The crust is the outer layer of our planet and ranges from 5  to 60 kilometers in depth. It is  the solid rock layer consisting of  continental crust and oceanic crust. Both the continental and oceanic crusts float on the upper mantle. The basic composition of the crust is represented by oxides of silicon, aluminum, iron, and other metals.

    <i><u>Continental crust</u></i>
    The <b>continental crust is</b> located under the continents and is from 30 to 60 kilometers thick. It is mostly composed of sedimentary, granitic and basaltic rocks. The mean thickness of sedimentary layer is 5.0 km, granit layer – 17 km, basalt layer - 22 km.

    <i><u>Oceanic crust</u></i>
    The <b>oceanic crust</b> extends to a depth of 3-10 kilometers under the ocean floor and forms at spreading centers on oceanic ridges. As opposed to continental crust, oceanic crust hasn’t granite layer and is composed only of basalt and sedimentary rocks.</align>";
                    default:
                        return "Error";
                }
            case Language.Russian:
                switch (query)
                {
                    case InformationsMockup.Earth:
                        //return "<align=justified>Планета Земля является третей планетой расположенной от солнца и пятой планетой по величине в солнечной системе. Она меньше чем Юпитер, Сатурн, Уран и Нептун, но крупнее Меркурия, Марса и Венеры. Радиус земли составляет 6371 км. Земля, это единственная на данный момент планета, на которой обнаружена жизнь, атмосфера, содержащая кислород, и вода, большая часть которой расположена в океанах. Примерно пятая часть атмосферы состоит из кислорода, вырабатываемого растениями.  Ученые считают, то земля образовалась в одно и тоже время, что и другие планеты  -  примерно 4.54 млрд лет назад. По химическому составу  земля состоит  из железа (32,1%), кислорода (30,1), кремния (15.1%),  магния (13.9%),  серы (2.9%),  никеля (1.8%), кальция (1.5%) и алюминия (1.4%). Содержание остальных элементов составляет 1,2%. На основании физических и химических свойств в Земле выделяются 3 главных слоя: кора, мантия и Ядро.</align>";
                        return @"<align=justified>  Планета Земля является третей планетой расположенной от солнца и пятой планетой по величине в солнечной системе. Она меньше чем Юпитер, Сатурн, Уран и Нептун, но крупнее Меркурия, Марса и Венеры. Радиус земли составляет 6371 км. Земля, это единственная на данный момент планета, на которой обнаружена жизнь, атмосфера, содержащая кислород, и вода, большая часть которой расположена в океанах. Примерно пятая часть атмосферы состоит из кислорода, вырабатываемого растениями.  Ученые считают, то земля образовалась в одно и тоже время, что и другие планеты  -  примерно 4.54 млрд лет назад. По химическому составу  земля состоит  из железа (32.1%), кислорода (30.1), кремния (15.1%),  магния (13.9%),  серы (2.9%),  никеля (1.8%), кальция (1.5%) и алюминия (1.4%). Содержание остальных элементов составляет 1.2%. На основании физических и химических свойств в Земле выделяются 3 главных слоя: кора, мантия и Ядро. </align>

<align=left>    <b>Основные сведения: </b>
<b>Средний радиус Земли</b> – 6371 км.
<b>Масса</b> - 5.97237x10<sup>24</sup> кг
<b>Объем</b>  - 1.08321x1012 км<sup>3</sup>
<b>Средняя плотность</b>  – 5.52 г/см<sup>3</sup>
</align>
";
                    case InformationsMockup.Crust:
                        //return "<align=justified>Земная кора является самым верхнем слоем нашей планеты и  как бы “плавает” на верхней мантии.  Ее мощность колеблется от 5 км, под океанами и достигает 60 км под континентами. Это связано с различным строением континентальной и океанической коры. Континентальная кора состоит из 3-х слоев: осадочного, гранитного и базальтового. Океаническая кора образуется в зонах спрединга, вдоль океанических хребтов. Она состоит из двух слоев: осадочного и базальтового. Поверхность земной коры довольно хорошо изучена человеком. Самой глубокой скважиной, пробуренной на поверхности Земли до недавних пор являлась  Кольская сверхглубокая скважина. Ее глубина достигла более 12 километров!";
                        return @"<align=justified>  Земная кора является самым верхнем слоем нашей планеты и  как бы “плавает” на верхней мантии.  Ее мощность колеблется от 5 км, под океанами и достигает 60 км под континентами. Это связано с различным строением континентальной и океанической коры. Континентальная кора состоит из 3-х слоев: осадочного, гранитного и базальтового. Океаническая кора образуется в зонах спрединга, вдоль океанических хребтов. Она состоит из двух слоев: осадочного и базальтового. Поверхность земной коры довольно хорошо изучена человеком. Самой глубокой скважиной, пробуренной на поверхности Земли до недавних пор являлась  Кольская сверхглубокая скважина. Ее глубина достигла более 12 километров! </align>

<align=left>    <b>Химический состав, %:</b>
SiО – 56.8
Al<sub>2</sub>O<sub>3</sub> – 14.8
FeO – 6.1
CaO – 8
Na<sub>2</sub>O – 2.7
K<sub>2</sub>O – 1.5
MgO – 5.1</align>";
                    case InformationsMockup.Mantle:
                    case InformationsMockup.UpperMantle:
                        //return "<align=justified>Мантия залегает под земной корой и имеет мощностью до двух тысяч девятьсот километров. Состав мантии определен на основе геофизических и экспериментальных данных и соответствует ультраосновным породам обогащенных магнием и железом.  Мантия составляет более двух третей всей массы земли и также делится на две части: нижнюю и верхнюю. Верхняя часть мантии, граничащая с земной корой называется литосферой. Она имеет мощность до 100 км и находится в твердом состоянии. Следующие 300 км мантии находятся в пластичном состоянии – это астеносфера,  источник магмы, которая поступает на поверхность земли через вулканы. Нижняя мантия залегает примерно на глубине в 660 км от поверхности земли и заканчивается на глубине  в 2900 км, на гранит с ядром. Она находится в твердом состоянии и имеет температуру около 3000 °C, значительно выше, чем в верхней мантии. Высокое давление, созданное вышележащими породами не дает ей расплавиться.</align>";
                    case InformationsMockup.LowerMantle:
                        //return "<align=justified>Мантия залегает под земной корой и имеет мощностью до двух тысяч девятьсот километров. Состав мантии определен на основе геофизических и экспериментальных данных и соответствует ультраосновным породам обогащенных магнием и железом.  Мантия составляет более двух третей всей массы земли и также делится на две части: нижнюю и верхнюю. Верхняя часть мантии, граничащая с земной корой называется литосферой. Она имеет мощность до 100 км и находится в твердом состоянии. Следующие 300 км мантии находятся в пластичном состоянии – это астеносфера,  источник магмы, которая поступает на поверхность земли через вулканы. Нижняя мантия залегает примерно на глубине в 660 км от поверхности земли и заканчивается на глубине  в 2900 км, на гранит с ядром. Она находится в твердом состоянии и имеет температуру около 3000 °C, значительно выше, чем в верхней мантии. Высокое давление, созданное вышележащими породами не дает ей расплавиться.</align>";
                    case InformationsMockup.UpperMantleLess:
                    case InformationsMockup.LowerMantleLess:
                        //return "<align=justified>Мантия залегает под земной корой и имеет мощностью до двух тысяч девятьсот километров. Состав мантии определен на основе геофизических и экспериментальных данных и соответствует ультраосновным породам обогащенных магнием и железом.  Мантия составляет более двух третей всей массы земли и также делится на две части: нижнюю и верхнюю. Верхняя часть мантии, граничащая с земной корой называется литосферой. Она имеет мощность до 100 км и находится в твердом состоянии. Следующие 300 км мантии находятся в пластичном состоянии – это астеносфера,  источник магмы, которая поступает на поверхность земли через вулканы. Нижняя мантия залегает примерно на глубине в 660 км от поверхности земли и заканчивается на глубине  в 2900 км, на гранит с ядром. Она находится в твердом состоянии и имеет температуру около 3000 °C, значительно выше, чем в верхней мантии. Высокое давление, созданное вышележащими породами не дает ей расплавиться.</align>";
                        return @"<align=justified>  Мантия залегает под земной корой и имеет мощностью до двух тысяч девятьсот километров. Состав мантии определен на основе геофизических и экспериментальных данных и соответствует ультраосновным породам обогащенных магнием и железом.  Мантия составляет более двух третей всей массы земли и также делится на две части: нижнюю и верхнюю. Верхняя часть мантии, граничащая с земной корой называется литосферой. Она имеет мощность до 100 км и находится в твердом состоянии. Следующие 300 км мантии находятся в пластичном состоянии – это астеносфера,  источник магмы, которая поступает на поверхность земли через вулканы. Нижняя мантия залегает примерно на глубине в 660 км от поверхности земли и заканчивается на глубине  в 2900 км, на гранитце с ядром. Она находится в твердом состоянии и имеет температуру около 3000<sup>o</sup>C, значительно выше, чем в верхней мантии. Высокое давление, созданное вышележащими породами не дает ей расплавиться. </align>

<align=left>    <b>Химический состав, %:</b>
SiО – 45.3
MgO – 41.3
FeO – 7.3
Al<sub>2</sub>O<sub>3</sub> – 3.6
CaO – 1.9</align>";
                    case InformationsMockup.InnerCore:
                    case InformationsMockup.OuterCore:
                        //return "<align=justified>Earths Core is extends to a depth of around 2900 km  composed mainly of an iron and nickel alloy. The core is divided into two different zones. The outer core is a liquid because the temperatures there are adequate to melt the iron-nickel alloy. However, the inner core is a solid even though its temperature is higher than the outer core. Here, tremendous pressure, produced by the weight of the overlying rocks is strong enough to crowd the atoms tightly together and prevents the liquid state.  The core is earths source of internal heat because it contains radioactive materials which release heat as they break down into more stable substances.</align>";
                        return @"<align=justified>  На основании химического состава и физических свойств, в строении земли выделяются три основных слоя: земная кора, мантия и ядро.
 	Ядро Земли имеет радиус около 3470 км и состоит в основном из железоникелевого сплава. Оно делится на 2 различные зоны: внутреннюю и внешнюю. Внешнее ядро находится в жидком состоянии в следствии высокой температуры, способной расплавить даже железоникелевый сплав. Однако внутреннее ядро находится в твердом состоянии не смотря на то, что его температура еще выше. Это связано с гигантским  давлением, созданным вышележащими породами. Оно сдавливает атомы металлов более плотно и не позволяет внутреннему ядру расплавиться. Ядро является источником земного тепла, которое вырабатывается в результате разложения радиоактивных металлов в более стабильные изотопы. </align>

<align=left>    <b>Химический состав, %:</b>
Fe - 80
Ni – 9
Si - 7
O – 4
S – 2</align>";
                    case InformationsMockup.Sandstone:
                        //return "The mean thickness of sedimentary rocks ranges from 0 to 15 km. The most widespread sedimentary rocks are sand, clay, mud and limy mud. Limestones were once carbonate mud on the seafloor. This mud is made of tiny carbonate shells. Sandstone and claystone is a lithified sand and clays corresponding. Mean chemical composition is represented mostly by oxides of silicon (44%), calcium (16%), aluminium (11%) and magnesium (3%).";
                        //return "Средняя мощность осадочного слоя варьирует от 0 до 15 км (в среднем 5 км). Наиболее распространенными осадочными породами на нашей планете являются песок, глина и карбонатные породы, например известняк. Когда песок и глина опускаются на глубину, то под воздействием давления они превращаются в песчаник и сланцы соответственно! Усредненный химический состав осадочного слоя  представлен на 44% кремнием, на 16% кальцием, на 11% алюминием и на 3% магнием.";
                        return @"<align=justified>  Средняя мощность осадочного слоя варьирует от 0 до 15 км (в среднем 5 км). Наиболее распространенными осадочными породами на нашей планете являются песок, глина и карбонатные породы, например известняк. Когда песок и глина опускаются на глубину, то под воздействием давления они превращаются в песчаник и сланцы соответственно! Усредненный химический состав осадочного слоя  представлен на 44% кремнием, на 16% кальцием, на 11% алюминием и на 3% магнием. </align>";
                    case InformationsMockup.Granit:
                        //return "Гранитный слой характерен только для континентального типа коры и в основном состоит из гранитов, гнейсов и риолитов (вулканический аналог гранита). Его мощность варьирует от 10 до 40 километров (в среднем 17 км). Состав породообразующих оксидов отличается от осадочного слова повышенным содержанием  кремнезема(63%) и глинозема (15%), а так же пониженным содержанием оксида кальция (4%).";
                        return @"<align=justified>  Гранитный слой характерен только для континентального типа коры и в основном состоит из гранитов, гнейсов и риолитов (вулканический аналог гранита). Его мощность варьирует от 10 до 40 километров (в среднем 17 км). Состав породообразующих оксидов отличается от осадочного слова повышенным содержанием  кремнезема(63%) и глинозема (15%), а так же пониженным содержанием оксида кальция (4%).</align>";
                    case InformationsMockup.Basalt:
                        //return "The mean thickness of basalt layer is about 22 km. Basalt layer of continental crust has very diverse composition with a predominance of metamorphic basic rocks such as granulites, eclogites. Basalt layer of the oceanic crust is much more homogeneous. The predominant rock type is so-called tholeiitic basalt which differs  from continental basalts by low content of potassium, rubidium, strontium, barium and uranium. The main oxides of the layer is silica (52%), aluminium (15%), calcium (9%), magnesium (7%) and iron (6%).";
                        //return "Базальтовый слой имеет среднюю мощность около 22 километров. Базальтовый слой континентального типа имеет более сложный состав по сравнению с океаническим и состоит в основном  из метаморфических пород, таких как гранулиты и  эклогиты. Базальтовый слой океанической коры более однороден и состоит в основном из так называемых толеитовых базальтов, для которых характерно пониженное содержание калия, рубидия, стронция, бария и урана. Среднее содержание основных оксидов: SiО - 52%, Al2O3 - 15%, CaO - 9%, MgO -7%, Fe2O3 -6%.";
                        return @"<align=justified>  Базальтовый слой имеет среднюю мощность около 22 километров. Базальтовый слой континентального типа имеет более сложный состав по сравнению с океаническим и состоит в основном  из метаморфических пород, таких как гранулиты и  эклогиты. Базальтовый слой океанической коры более однороден и состоит в основном из так называемых толеитовых базальтов, для которых характерно пониженное содержание калия, рубидия, стронция, бария и урана. </align>

<align=left>    <b>Среднее содержание основных оксидов, %: </b>
SiО - 52
Al<sub>2</sub>O<sub>3</sub> - 15
CaO - 9
MgO -7
Fe<sub>2</sub>O<sub>3</sub> -6</align>";
                    case InformationsMockup.OceanicCrust:
                        //return "Океаническая кора  имеет мощность в 3-10 км, залегает под океанами и образуется в зонах спрединга, вдоль океанических хребтов. В отличии от континентальной коры, у океанической коры отсутствует гранитный слой и она состоит только из осадочного и базальтового слоев.";
                        return @"<align=justified>   Океаническая кора  имеет мощность в 3-10 км, залегает под океанами и образуется в зонах спрединга, вдоль океанических хребтов. В отличии от континентальной коры, у океанической коры отсутствует гранитный слой и она состоит только из осадочного и базальтового слоев. </align>";
                    case InformationsMockup.ContinentalCrust:
                        //return "Для континентального типа коры характерна значительаня мощность (от 30 до 60 км) и присутствие гранитного слоя. В целом она состоит из 3-х слоев: осадочного, гранитного и базальтового. Средняя мощность осадочного слоя составляет 5,0 км, гранитного — 17, базальтового — около 22 км .";
                        return @"<align=justified>   Для континентального типа коры характерна значительная мощность (от 30 до 60 км) и присутствие гранитного слоя. В целом она состоит из 3-х слоев: осадочного, гранитного и базальтового. Средняя мощность осадочного слоя составляет 5.0 км, гранитного — 17, базальтового — около 22 км. </align>";
                    case InformationsMockup.MagneticField:
                        //return "Магнитное поле Земли генерируется токами в жидком металлическом ядре и является основным барьером, защищающем ее от солнечного ветра и радиации в том числе, создавая вокруг земли магнитосферу. В случае взаимодействия магнитного поля с солнечным ветром, магнитосфера приобретает достаточно сложную форму, обтекаемую солнечным ветром. Магнитные полюса находятся в постоянном движении, в результате чего они периодически меняются местами. Последняя перемена полюсов местами произошла примерно 700 000 лет назад, а за последние 10 млн лет полярность менялась 18 раз!";
                        return @"<align=justified>  Магнитное поле Земли генерируется токами в жидком металлическом ядре и является основным барьером, защищающем ее от солнечного ветра и радиации в том числе, создавая вокруг земли магнитосферу. В случае взаимодействия магнитного поля с солнечным ветром, магнитосфера приобретает достаточно сложную форму, обтекаемую солнечным ветром. Магнитные полюса находятся в постоянном движении, в результате чего они периодически меняются местами. Последняя перемена полюсов местами произошла примерно 700 000 лет назад, а за последние 10 млн лет полярность менялась 18 раз! </align>";
                    case InformationsMockup.Temperature:
                        //return "Средняя температура нашей планеты равна 15 ºС. С начала ХХ столетия средняя температура воздуха возросла на 5%,  примерно две трети приходятся на период после 80-х гг прошлого столетия. При сохранении текущей тенденции, уже к концу нашего столетия глобальное потепление может привести к катастрофическим последствиям. Основной причиной потепления служит активная индустриализация человечества. Наивысшая температура, зафиксированная в 1913 году в Калифорнии, составила 56,7 ºС. Минимальная температура, зафиксированная на станции Восток в 1983 году составила -89,2 ºС.";
                        return @"<align=justified>  Средняя температура нашей планеты равна 15 <sup>o</sup>. С начала ХХ столетия средняя температура воздуха возросла на 5%,  примерно две трети приходятся на период после 80-х гг прошлого столетия. При сохранении текущей тенденции, уже к концу нашего столетия глобальное потепление может привести к катастрофическим последствиям. Основной причиной потепления служит активная индустриализация человечества. Наивысшая температура, зафиксированная в 1913 году в Калифорнии, составила 56.7 <sup>o</sup>С. Минимальная температура, зафиксированная на станции Восток в 1983 году составила -89.2 <sup>o</sup>С. </align>";
                    case InformationsMockup.Mass:
                        //return "Масса земли равна 5.97237x1024 кг. Для сравнения, масса Луны примерно в 81 раз меньше массы земли. В то время как Юпитер более чем в 300 раз тяжелее Земли.   Масса Земли не является постоянной величиной и постоянно меняется. В настоящее время потеря массы превышает прирост. Основной причиной прироста земной массы ( ~ 40 000 т/г) являются космическая пыль и метеоры. Основным фактором уменьшающим массу земли ( ~ 96600 т/г) является потеря газов (в основном водорода и гелия) атмосферой планеты вследствие их рассеяния в космическое пространство.";
                        return @"<align=justified>  Масса земли равна 5.97237x10<sup>24</sup>  кг. Для сравнения, масса Луны примерно в 81 раз меньше массы земли. В то время как Юпитер более чем в 300 раз тяжелее Земли.   Масса Земли не является постоянной величиной и постоянно меняется. В настоящее время потеря массы превышает прирост. Основной причиной прироста земной массы ( ~ 40 000 т/г) являются космическая пыль и метеоры. Основным фактором уменьшающим массу земли ( ~ 96600 т/г) является потеря газов (в основном водорода и гелия) атмосферой планеты вследствие их рассеяния в космическое пространство. </align>";
                    case InformationsMockup.Pie:
                        return @"<align=justified>  Земная кора является самым верхнем слоем Земли и имеет мощность от 5 до 60 км. Это твердая оболочка земли, состоящая из океанической и континентальной коры.  Оба типа коры залегают поверх мантии. Средний химический состав коры представлен оксидами кремния, алюминия, железа и других металлов.

    <i><u>Континентальная кора</u></i>
    Для континентального типа коры характерна значительная мощность (от 30 до 60 км) и присутствие гранитного слоя. В целом она состоит из 3-х слоев: осадочного, гранитного и базальтового. Средняя мощность осадочного слоя составляет 5.0 км, гранитного — 17, базальтового — около 22 км.

    <i><u>Океаническая кора</u></i>
    Океаническая кора  имеет мощность в 3-10 км, залегает под океанами и образуется в зонах спрединга, вдоль океанических хребтов. В отличии от континентальной коры, у океанической коры отсутствует гранитный слой и она состоит только из осадочного и базальтового слоев.</align>";
                    default:
                        return "Error";
                }
        }

        return "";
    }
}