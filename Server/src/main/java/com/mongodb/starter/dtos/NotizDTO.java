package com.mongodb.starter.dtos;

import com.mongodb.starter.models.Notiz;
import org.bson.types.ObjectId;


public record  NotizDTO(String id, String title, String text) {

    public NotizDTO(Notiz n) {
        this(n.getId() == null ? new ObjectId().toHexString() : n.getId().toHexString(), n.getTitle(), n.getText());
    }

    public Notiz toNotiz() {
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new Notiz(_id, title, text);
    }
}
