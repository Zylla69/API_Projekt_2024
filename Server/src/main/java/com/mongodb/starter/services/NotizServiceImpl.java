package com.mongodb.starter.services;

import com.mongodb.starter.dtos.NotizDTO;
import com.mongodb.starter.models.Notiz;
import com.mongodb.starter.repositories.NotizRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class NotizServiceImpl implements NotizService {

    private final NotizRepository notizRepository;

    public NotizServiceImpl(NotizRepository notizRepository) {
        this.notizRepository = notizRepository;
    }

    @Override
    public NotizDTO save(NotizDTO NotizDTO) {
        return new NotizDTO(notizRepository.save(NotizDTO.toNotiz()));
    }


    @Override
    public List<NotizDTO> findAll() {
        return notizRepository.findAll().stream().map(NotizDTO::new).toList();
    }

    @Override
    public List<NotizDTO> findAll(List<String> ids) {
        return notizRepository.findAll(ids).stream().map(NotizDTO::new).toList();
    }

    @Override
    public NotizDTO findOne(String id) {
        return new NotizDTO(notizRepository.findOne(id));
    }


    @Override
    public long delete(String id) {
        return notizRepository.delete(id);
    }

    @Override
    public NotizDTO update(NotizDTO NotizDTO) {
        return new NotizDTO(notizRepository.update(NotizDTO.toNotiz()));
    }

}
