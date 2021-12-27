mod pattern_match_iterator;
mod buffered_iterator;
mod reader_match_iterator;

use std::io::Read;
use super::model::Pattern;
use pattern_match_iterator::PatternMatchIterator;
use buffered_iterator::BufferedSliceIterator;
use reader_match_iterator::ReaderMatchIterator;

const DEFAULT_BUFFER_SIZE: usize = 8*1024*1024;

pub fn matches_from_slice<'a, OU: Into<Option<usize>>>(pattern: &'a Pattern, bytes: &'a [u8], start_pos: OU) -> PatternMatchIterator<'a> {
    if pattern.is_empty() {
        panic!("Cannot match on empty pattern!")
    }

    PatternMatchIterator::new(bytes, pattern, start_pos.into().unwrap_or_default())
}

pub fn matches_from_reader<'a, R: Read, OU: Into<Option<usize>>>(pattern: &'a Pattern, reader: R, buffer_size: OU) -> ReaderMatchIterator<'a, R> {
    if pattern.is_empty() {
        panic!("Cannot match on empty pattern!")
    }

    let bsi = BufferedSliceIterator::new(reader, pattern.len(), 1, buffer_size.into().unwrap_or(DEFAULT_BUFFER_SIZE));
    ReaderMatchIterator::new(pattern, bsi)
}
