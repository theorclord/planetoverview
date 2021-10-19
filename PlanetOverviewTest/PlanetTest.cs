using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanetOverview.Data;
using PlanetOverview.GameComponents;
using PlanetOverview.PlanetComponents;
using PlanetOverview.PlayerComponents;
using PlanetOverview.UnitComponents;
using System.Collections.Generic;

namespace PlanetOverviewTest
{
    [TestClass]
    public class PlanetTest
    {
        [TestMethod]
        public void StructureAddNewTest()
        {
            GameContainer container = new GameContainer();
            container.LoadFactionsJson();
            // Create new planet. Add Owner to it. 
            Planet testPlanet = new Planet
            {
                Owner = new Player() { Name = "TestOwner", Faction = container.IDFactions[AllConstants.EmpireStringID], Credits = 100000 },
                SupportedGroundStructureAmount = 1,
            };


            // Add structure
            List<(bool, Structure)> buildableStrucs = testPlanet.GetBuildableStructures();

            Assert.IsTrue(buildableStrucs.Count > 0);

            testPlanet.AddNewStructure(new Structure(buildableStrucs[0].Item2));

            // Check the unit is added and is a new unit
            Assert.IsTrue(testPlanet.PlanetStructures.Count == 1);
            Assert.AreNotEqual(buildableStrucs[0].Item2, testPlanet.PlanetStructures[0]);
            Assert.AreEqual(buildableStrucs[0].Item2.Name, testPlanet.PlanetStructures[0].Name);

            // Since the build slot have been finished, there should be no structures available
            Assert.IsFalse(testPlanet.GetBuildableStructures().Count > 0);
        }

        [TestMethod]
        public void StructureAddNewNoCredits()
        {
            GameContainer container = new GameContainer();
            container.LoadFactionsJson();
            // Create new planet. Add Owner to it. 
            Planet testPlanet = new Planet
            {
                Owner = new Player() { Name = "TestOwner", Faction = container.IDFactions[AllConstants.EmpireStringID], Credits = 0 },
                SupportedGroundStructureAmount = 1,
            };


            // Add structure
            List<(bool, Structure)> buildableStrucs = testPlanet.GetBuildableStructures();

            Assert.IsTrue(buildableStrucs.Count > 0);

            if (buildableStrucs[0].Item1)
            {
                testPlanet.AddNewStructure(new Structure(buildableStrucs[0].Item2));
            }

            // Check the structure haven't been added
            Assert.IsTrue(testPlanet.PlanetStructures.Count == 0);

            // Sine no structure have been added, there should the same amount of buildable structres
            Assert.AreEqual(testPlanet.GetBuildableStructures().Count, buildableStrucs.Count);
        }
    }
}
