﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuiaTBAWP.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int max = 4000;

            var list = DataColectivos.LoadBusesIUrb();
            foreach (var bus in list.Where(bus => bus.Description.Length > max))
            {
                i++;
                Console.WriteLine(bus.Title + " | " + bus.Description.Length);
            }

            list = DataColectivos.LoadBusesMuni();
            foreach (var bus in list.Where(bus => bus.Description.Length > max))
            {
                j++;
                Console.WriteLine(bus.Title + " | " + bus.Description.Length);
            }

            list = DataColectivos.LoadBusesProv();
            foreach (var bus in list.Where(bus => bus.Description.Length > max))
            {
                k++;
                Console.WriteLine(bus.Title + " | " + bus.Description.Length);
            }

            Console.WriteLine(string.Format("TOTAL IU: {0}", i));
            Console.WriteLine(string.Format("TOTAL MU: {0}", j));
            Console.WriteLine(string.Format("TOTAL PR: {0}", k));

        }
    }
}
