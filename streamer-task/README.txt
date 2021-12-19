Roy Weisberger

Instructions how to run tests:
------------------------------
1. Extract the test data file (streamer_test_data.zip) in your test data folder.
2. Change the value of the variable TEST_DATA_PATH in the GlobalData.cs file to your test data folder.
3. Run "Build->Rebuild Solution"
4. Run "Test->Run->All Tests"
5. In order to test large files uncomment the TestLargeFile method in the IntegrationTest.cs file and change the file
		name to your large file name.