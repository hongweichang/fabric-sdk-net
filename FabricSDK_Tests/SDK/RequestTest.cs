/*
 *
 *  Copyright 2016,2017 DTCC, Fujitsu Australia Software Technology, IBM - All Rights Reserved.
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *     http://www.apache.org/licenses/LICENSE-2.0
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */

using System;
using System.IO;
using Hyperledger.Fabric.SDK;
using Hyperledger.Fabric.SDK.Requests;
using Hyperledger.Fabric.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperledger.Fabric.Tests.SDK
{
    [TestClass]
    [TestCategory("SDK")]
    public class RequestTest
    {
        private readonly string someFileLocation = "empty";
        private readonly string someFileLocation2 = "blah";
        private HFClient hfclient;
        private Stream mockstream;

        [TestInitialize]
        public void SetupClient()
        {
            hfclient = HFClient.Create();
            mockstream = new MemoryStream();
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Chaincode META-INF location may not be set with chaincode input stream set.")]
        public void TestinstallProposalRequestStreamWithMeta()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeInputStream = mockstream;
            installProposalRequest.ChaincodeMetaInfLocation = someFileLocation;
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Error setting chaincode location. Chaincode input stream already set. Only one or the other maybe set.")]
        public void TestinstallProposalRequestStreamWithSourceLocation()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeInputStream = mockstream;
            Assert.AreEqual(installProposalRequest.ChaincodeInputStream, mockstream);
            installProposalRequest.ChaincodeSourceLocation = someFileLocation;
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Error setting chaincode input stream. Chaincode source location already set. Only one or the other maybe set.")]
        public void TestinstallProposalRequestWithLocationSetStream()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeSourceLocation = someFileLocation;
            installProposalRequest.ChaincodeInputStream = mockstream;
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Error setting chaincode input stream. Chaincode META-INF location  already set. Only one or the other maybe set.")]
        public void TestinstallProposalRequestWithMetaInfSetStream()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeMetaInfLocation = someFileLocation;
            installProposalRequest.ChaincodeInputStream = mockstream;
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Chaincode META-INF location may not be null.")]
        public void TestinstallProposalRequestWithMetaInfSetStreamNULL()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();
            installProposalRequest.ChaincodeMetaInfLocation = null;
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Chaincode source location may not be null")]
        public void TestinstallProposalRequestWithSourceNull()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeSourceLocation = null;
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Chaincode input stream may not be null.")]
        public void TestinstallProposalRequestWithInputStreamNULL()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeInputStream = null;
        }

        [TestMethod]
        public void TestinstallProposalRequestLocationAndMeta()
        {
            InstallProposalRequest installProposalRequest = hfclient.NewInstallProposalRequest();

            installProposalRequest.ChaincodeSourceLocation = someFileLocation;
            installProposalRequest.ChaincodeMetaInfLocation = someFileLocation2;

            Assert.AreEqual(installProposalRequest.ChaincodeSourceLocation, someFileLocation);
            Assert.AreEqual(installProposalRequest.ChaincodeMetaInfLocation, someFileLocation2);
        }
    }
}