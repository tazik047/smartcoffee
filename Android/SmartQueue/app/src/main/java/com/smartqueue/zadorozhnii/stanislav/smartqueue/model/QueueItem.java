package com.smartqueue.zadorozhnii.stanislav.smartqueue.model;

import android.widget.LinearLayout;
import android.widget.TextView;

import com.smartqueue.zadorozhnii.stanislav.smartqueue.MyApplication;
import com.smartqueue.zadorozhnii.stanislav.smartqueue.infrastructure.AccountHelper;

import java.util.List;
import java.util.Objects;

/**
 * Created by Tazik on 04.01.2016.
 */
public class QueueItem {
    public String timeToEnd;

    public List<UserQueue> users;

    public void GenerateList(LinearLayout layout){
        String currentId = String.valueOf(AccountHelper.getInstance().getCurrentUser().id);
        layout.removeAllViews();
        int ind = 1;
        for (UserQueue i : users ) {
            TextView text = new TextView(MyApplication.getContext());
            text.setText(String.format("%d. %s %s %s", ind++, i.surname, i.name, i.id.equals(currentId)?"(Вы)":""));
            layout.addView(text);
        }
    }

    @Override
    public boolean equals(Object o) {
        if(o==null){
            return false;
        }
        if(! (o instanceof QueueItem)){
            return false;
        }
        QueueItem item = (QueueItem)o;
        int j =0;
        if(item.users.size()!=users.size()){
            return false;
        }
        for (UserQueue i: item.users){
            if(!users.get(j++).id.equals(i.id)){
                return false;
            }
        }
        return true;
    }
}

class UserQueue{
    public String id;

    public String name;

    public String surname;
}
