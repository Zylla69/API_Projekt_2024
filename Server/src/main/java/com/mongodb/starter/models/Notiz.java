package com.mongodb.starter.models;

import org.bson.types.ObjectId;

public class Notiz {
    ObjectId id;
    String title;
    String text;

    public Notiz(){ }

    public Notiz(ObjectId id, String title, String text){
         this.id = id;
         this.title = title;
         this.text = text;
    }

    public ObjectId getId(){
        return id;
    }

    public void setId(ObjectId id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }
}
