package com.mongodb.starter.repositories;

import com.mongodb.starter.models.Notiz;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface NotizRepository {

    Notiz save(Notiz notiz);

    List<Notiz> findAll();

    List<Notiz> findAll(List<String> ids);

    Notiz findOne(String id);

    long delete(String id);
    Notiz update(Notiz notiz);
}
