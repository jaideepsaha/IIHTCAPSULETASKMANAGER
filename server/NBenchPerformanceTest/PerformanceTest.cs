﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBench;
using NBench.Util;
using NBench.Collection.Memory;
using TaskWebAPI.Controllers;
using TaskDL;
using System.Web.Http;


namespace NBenchPerformanceTest
{
    public class PerformanceTest
    {
        TaskDetailNewController taskWebAPI = new TaskDetailNewController();
        TaskDetail item;
        private const int AcceptableMinAddThroughput = 20000000;
        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void MeasureGarbageCollections()
        {
            RunTest();
        }
        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test)]
        [GcThroughputAssertion(GcMetric.TotalCollections, GcGeneration.Gen0, MustBe.LessThan, 300)]
        [GcThroughputAssertion(GcMetric.TotalCollections, GcGeneration.Gen1, MustBe.LessThan, 150)]
        [GcThroughputAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.LessThan, 20)]
        [GcTotalAssertion(GcMetric.TotalCollections, GcGeneration.Gen2, MustBe.LessThan, 50)]
        public void TestGarbageCollections()
        {
            RunTest();
        }
        private readonly List<int[]> dataCache = new List<int[]>();
        private void RunTest()
        {
            for (var i = 0; i < 500; i++)
            {
                item = new TaskDetail();
                item.Parent_Id = 2;
                item.Range = 10;
                item.Start_Date = DateTime.Today;
                item.End_Date = DateTime.Today;
                taskWebAPI.Post(item);

            }
        }
        public void AddThroughput_IterationsMode(BenchmarkContext context)
        {
            for (var i = 0; i < AcceptableMinAddThroughput; i++)
            {
                item = new TaskDetail();
                item.Parent_Id = 2;
                item.Range = 10;
                item.Start_Date = DateTime.Today;
                item.End_Date = DateTime.Today;
                taskWebAPI.Post(item);
            }
        }
    }
}
