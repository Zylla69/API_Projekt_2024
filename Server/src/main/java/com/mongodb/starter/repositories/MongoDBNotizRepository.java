package com.mongodb.starter.repositories;

import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.FindOneAndReplaceOptions;
import com.mongodb.starter.models.Notiz;
import jakarta.annotation.PostConstruct;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;
import static com.mongodb.client.model.Filters.*;
import static com.mongodb.client.model.ReturnDocument.AFTER;

@Repository
public class MongoDBNotizRepository implements NotizRepository {

    private static final TransactionOptions txnOptions = TransactionOptions.builder()
                                                                           .readPreference(ReadPreference.primary())
                                                                           .readConcern(ReadConcern.MAJORITY)
                                                                           .writeConcern(WriteConcern.MAJORITY)
                                                                           .build();
    private final MongoClient client;
    private MongoCollection<Notiz> notizCollection;

    public MongoDBNotizRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        notizCollection = client.getDatabase("POSProjekt").getCollection("notizen", Notiz.class);
    }

    @Override
    public Notiz save(Notiz notiz) {
        notiz.setId(new ObjectId());
        notizCollection.insertOne(notiz);
        return notiz;
    }

    @Override
    public List<Notiz> findAll() {
        return notizCollection.find().into(new ArrayList<>());
    }

    @Override
    public List<Notiz> findAll(List<String> ids) {
        return notizCollection.find(in("_id", mapToObjectIds(ids))).into(new ArrayList<>());
    }

    @Override
    public Notiz findOne(String id) {
        return notizCollection.find(eq("_id", new ObjectId(id))).first();
    }

    @Override
    public long delete(String id) {
        return notizCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }

    @Override
    public Notiz update(Notiz notiz) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        return notizCollection.findOneAndReplace(eq("_id", notiz.getId()), notiz, options);
    }

    private List<ObjectId> mapToObjectIds(List<String> ids) {
        return ids.stream().map(ObjectId::new).toList();
    }
}
