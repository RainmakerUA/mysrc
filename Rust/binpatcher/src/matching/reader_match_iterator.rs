use super::BufferedSliceIterator;
use super::Pattern;
use std::io::Read;

pub struct ReaderMatchIterator<'a, R: Read> {
    pattern: &'a Pattern,
    iterator: BufferedSliceIterator<R>,
}

impl<'a, R: Read> ReaderMatchIterator<'a, R> {
    pub fn new(pattern: &'a Pattern, bsi: BufferedSliceIterator<R>) -> ReaderMatchIterator<'a, R> {
        ReaderMatchIterator {
            pattern,
            iterator: bsi,
        }
    }
}

impl<'a, R: Read> Iterator for ReaderMatchIterator<'a, R> {
    type Item = usize;

    fn next(&mut self) -> Option<Self::Item> {
        for (pos, slice) in self.iterator.by_ref() {
            if self.pattern.is_match(&slice).unwrap() {
                return Some(pos);
            }
        }
        None
    }
}
