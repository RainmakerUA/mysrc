use super::Pattern;
use super::BufferedSliceIterator;
use std::io::Read;

pub struct ReaderMatchIterator<'a, R: Read> {
    pattern: &'a Pattern,
    iterator: BufferedSliceIterator<R>,
}

impl <'a, R: Read> ReaderMatchIterator<'a, R> {
    pub fn new(pattern: &'a Pattern, bsi: BufferedSliceIterator<R>) -> ReaderMatchIterator<'a, R> {
        ReaderMatchIterator { pattern, iterator: bsi }
    }
}

impl <'a, R: Read> Iterator for ReaderMatchIterator<'a, R> {
    type Item = usize;

    fn next(&mut self) -> Option<Self::Item> {
        loop {
            if let Some((pos, slice)) = self.iterator.next() {
                if self.pattern.is_match(&*slice).unwrap() {
                    return Some(pos);
                }
            } else {
                break;
            }
        }
        None
    }
}
