use super::Pattern;

pub struct PatternMatchIterator<'a> {
    source: &'a [u8],
    pattern: &'a Pattern,
    position: usize,
}

impl<'a> PatternMatchIterator<'a> {
    pub fn new(source: &'a [u8], pattern: &'a Pattern, start_pos: usize) -> PatternMatchIterator<'a> {
        PatternMatchIterator { source: source, pattern: pattern, position: start_pos }
    }
}

impl <'a> Iterator for PatternMatchIterator<'a> {
    type Item = usize;

    fn next(&mut self) -> Option<Self::Item> {
        let pattern_len = self.pattern.len();
        let mut current_pos = self.position;
        while current_pos + pattern_len < self.source.len() {
            let window = &self.source[current_pos .. current_pos + pattern_len];
            if self.pattern.is_match(window).unwrap() {
                self.position = current_pos + pattern_len;
                return Some(current_pos);
            }
            current_pos += 1;
        }
        self.position = current_pos;
        None
    }
}
