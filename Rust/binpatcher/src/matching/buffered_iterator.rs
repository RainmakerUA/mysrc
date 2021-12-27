
use std::io::{ Read, Result };

pub struct BufferedSliceIterator<R: Read> {
    reader: R,
    buffer: Vec<u8>,
    slice_size: usize,
    advance_size: usize,
    buffer_size: usize,
    read_len: usize,
    reader_pos: usize,
    buffer_pos: usize,
}

impl <R: Read> BufferedSliceIterator<R> {    
    pub fn new(reader: R, slice_size: usize, advance_size: usize, buffer_size: usize) -> BufferedSliceIterator<R> {
        // TODO: Size validation
        BufferedSliceIterator {
            reader: reader,
            buffer: Vec::new(),
            slice_size: slice_size,
            advance_size: advance_size,
            buffer_size: buffer_size,
            read_len: 0,
            reader_pos: 0,
            buffer_pos: 0,
        }
    }

    pub fn pos(&self) -> usize {
        self.reader_pos + self.buffer_pos
    }

    pub fn slice(&self, start: usize) -> &[u8] {
        &self.buffer[start..start+self.slice_size]
    }

    pub fn boxed_slice(&self, start: usize) -> Box<[u8]> {
        self.slice(start).to_vec().into_boxed_slice()
    }

    fn fill_buffer(&mut self, start: usize) -> Result<usize> {
        self.reader.read(&mut self.buffer[start..])
    }
}

impl <R: Read> Iterator for BufferedSliceIterator<R> {
    type Item = (usize, Box<[u8]>);

    fn next(&mut self) -> Option<Self::Item> {        
        if self.buffer.len() == 0 {
            self.buffer.resize(self.buffer_size, 0);
            self.read_len = self.fill_buffer(0).ok()?;
            self.buffer_size = self.read_len;
        }

        loop {
            if self.buffer_size > 0 {
                if self.buffer_pos + self.slice_size <= self.buffer_size {
                    let old_buf_pos = self.buffer_pos;
                    self.buffer_pos += self.advance_size;
                    return Some((self.pos(), self.boxed_slice(old_buf_pos)));
                } else {
                    let remaining_len = self.buffer_size - self.buffer_pos;
                    self.buffer.copy_within(self.buffer_pos..self.buffer_size, 0);
                    self.reader_pos += self.read_len;
                    self.read_len = self.fill_buffer(remaining_len).ok()?;
                    self.buffer_size = self.read_len;
                    self.buffer_pos = 0;
                }
            } else {
                return None;
            }
        }
    }
}
