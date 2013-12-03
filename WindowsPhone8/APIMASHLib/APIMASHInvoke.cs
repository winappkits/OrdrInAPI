﻿using System;
using System.Collections.Generic;

/*
* LICENSE: https://raw.github.com/apimash/StarterKits/master/LicenseTerms-SampleApps%20.txt
*/

/*
 *
 *  A P I   M A S H
 *
 *  Update the classes here to invoke the RESTful API call(s)
 */


namespace APIMASHLib
{
    public delegate void APIMASHEventHandler(object sender, APIMASHEvent e);

    public class APIMASHInvoke
    {
        public event APIMASHEventHandler OnResponse;

        async public void Invoke<T>(string apiCall, IEnumerable<KeyValuePair<string, string>> data=null, string method="GET")
        {
            var apimashEvent = new APIMASHEvent();

            try
            {
                T response = await APIMASHMap.LoadObject<T>(apiCall, data, method);
                apimashEvent.Object = response;
                apimashEvent.Status = APIMASHStatus.SUCCESS;
                apimashEvent.Message = string.Empty;
            }
            catch (Exception e)
            {
                apimashEvent.Message = e.Message;
                apimashEvent.Object = null;
                apimashEvent.Status = APIMASHStatus.FAILURE;
            }

            OnResponse(this, apimashEvent);
        }
    }
}
