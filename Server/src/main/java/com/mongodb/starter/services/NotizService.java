package com.mongodb.starter.services;


import com.mongodb.starter.dtos.NotizDTO;

import java.util.List;

public interface NotizService {

    NotizDTO save(NotizDTO NotizDTO);

    List<NotizDTO> findAll();

    List<NotizDTO> findAll(List<String> ids);

    NotizDTO findOne(String id);

    long delete(String id);

    NotizDTO update(NotizDTO NotizDTO);
}
