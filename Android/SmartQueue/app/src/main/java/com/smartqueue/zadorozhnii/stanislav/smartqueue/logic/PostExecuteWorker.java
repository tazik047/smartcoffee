package com.smartqueue.zadorozhnii.stanislav.smartqueue.logic;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;

/**
 * Created by Tazik on 27.12.2015.
 */
public interface PostExecuteWorker {
    void execute(ApiCaller caller, HttpClient result, HttpResponse response);
}
