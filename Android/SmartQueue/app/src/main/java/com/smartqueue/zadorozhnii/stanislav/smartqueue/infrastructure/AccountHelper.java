package com.smartqueue.zadorozhnii.stanislav.smartqueue.infrastructure;

import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.MyApplication;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.ApiCaller;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.logic.DataBaseHelper;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.model.User;

import java.lang.reflect.Type;

/**
 * Created by Stanislav_Zadorozhni on 28-Dec-15.
 */
public class AccountHelper {
    private static AccountHelper instance;
    private DataBaseHelper _db;

    private AccountHelper(){
        _db = new DataBaseHelper(MyApplication.getContext());
    }

    public String getUserPhotoPath(User user){
        return "http://"+ ApiCaller.DOMAIN + ":4747/account/photo/" + user.id;
    }

    public User getCurrentUser(){
        SQLiteDatabase db = _db.getReadableDatabase();
        Cursor userCursor = db.rawQuery("select * from " + DataBaseHelper.TABLE, null);

        if(userCursor.getCount()==0){
            return null;
        }

        userCursor.moveToFirst();
        String userJson = userCursor.getString(userCursor.getColumnIndex(DataBaseHelper.COLUMN_USER));
        Gson g= new Gson();
        Type t = new TypeToken<User>(){}.getType();
        return g.fromJson(userJson, t);
    }

    public static AccountHelper getInstance(){
        if(instance==null){
            instance = new AccountHelper();
        }
        return instance;
    }
}
