/*
 *  Copyright 2016, 2017 DTCC, Fujitsu Australia Software Technology, IBM - All Rights Reserved.
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *    http://www.apache.org/licenses/LICENSE-2.0
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */


using System;
using System.IO;
using System.Linq;
using Hyperledger.Fabric.SDK;
using Hyperledger.Fabric.SDK.Security;
using Hyperledger.Fabric.Tests.SDK.Integration;
using Hyperledger.Fabric.Tests.SDK.TestUtils;

namespace Hyperledger.Fabric.Tests.SDK
{
    public class TestHFClient
    {

        internal FileInfo tempFile;
        HFClient hfClient;

        public TestHFClient(FileInfo tempFile, HFClient hfClient)
        {
            this.tempFile = tempFile;
            this.hfClient = hfClient;
        }

        public static HFClient Create()
        {

            HFClient hfclient = HFClient.Create();
            SetupClient(hfclient);
            return hfclient;
        }

        public static string GetHomePath()
        {
            return (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }

        public static void SetupClient(HFClient hfclient)
        {
            string tempdir = Path.GetTempPath();
            string filename = "teststore_" + Path.GetTempFileName() + ".properties";
            string fulltempname = Path.Combine(tempdir, filename);

            FileInfo tempFile = new FileInfo(fulltempname);
            string props = Path.Combine(GetHomePath(), "test.properties");
            if (File.Exists(props))
                File.Delete(props);
            FileInfo sampleStoreFile = new FileInfo(props);
            SampleStore sampleStore = new SampleStore(sampleStoreFile);

            //src/test/fixture/sdkintegration/e2e-2Orgs/channel/crypto-config/peerOrganizations/org1.example.com/users/Admin@org1.example.com/msp/keystore/

            //SampleUser someTestUSER = sampleStore.getMember("someTestUSER", "someTestORG");
            SampleUser someTestUSER = sampleStore.GetMember("someTestUSER", "someTestORG", "mspid", FindFileSk("fixture/sdkintegration/e2e-2Orgs/" + TestConfig.FAB_CONFIG_GEN_VERS + "/crypto-config/peerOrganizations/org1.example.com/users/Admin@org1.example.com/msp/keystore"), new FileInfo(Path.GetFullPath("fixture/sdkintegration/e2e-2Orgs/" + TestConfig.FAB_CONFIG_GEN_VERS + "/crypto-config/peerOrganizations/org1.example.com/users/Admin@org1.example.com/msp/signcerts/Admin@org1.example.com-cert.pem")));
            someTestUSER.MspId = "testMSPID?";

            hfclient.CryptoSuite = HLSDKJCryptoSuiteFactory.Instance.GetCryptoSuite();
            hfclient.UserContext = someTestUSER;
        }

        static FileInfo FindFileSk(string directorys)
        {

            DirectoryInfo directory = new DirectoryInfo(Path.GetFullPath(directorys));
            FileInfo[] matches = directory.EnumerateFiles().Where(a => a.Name.EndsWith("_sk")).ToArray();

            if (null == matches)
            {
                throw new System.Exception($"Matches returned null does {directory.FullName} directory exist?");
            }

            if (matches.Length != 1)
            {
                throw new SystemException($"Expected in {directory.FullName} only 1 sk file but found {matches.Length}");
            }

            return matches[0];

        }

        ~TestHFClient()
        {
            if (tempFile != null)
            {
                try
                {
                    tempFile.Delete();
                }
                catch (System.Exception e)
                {
                    // // now harm done.
                }
            }
        }
    }
}
