use super::{
            BytePart,
            ParseBytePartError,
            BytePartErrorKind,
            PatternError,
            PatternErrorKind
        };

type PatternMatchResult = Result<bool, PatternError>;
type PatternApplyResult = Result<(), PatternError>;

#[derive(Debug, Clone, Default, PartialEq, Eq)]
pub struct Pattern {
    parts: Vec<BytePart>
}

pub struct PatternMatches<'a> {
    source: &'a [u8],
    pattern: &'a Pattern,
    position: usize,
}

impl Pattern {
    pub fn parse(text: &str) -> Result<Self, ParseBytePartError> {
        let mut iter = text.bytes().filter(|b| !b.is_ascii_whitespace());
        let mut pattern = Pattern::new();

        loop {
            let first = iter.next();
            let second = iter.next();

            if let Some(first) = first {
                if let Some(second) = second {
                    pattern.parts.push(BytePart::parse(first, second)?);
                    continue;
                }
                else {
                    return Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength));
                }
            }
            break;
        }
        Ok(pattern)
    }

    pub fn new() -> Self {
        Pattern { parts: Vec::new() }
    }

    pub fn len(&self) -> usize {
        self.parts.len()
    }

    pub fn is_empty(&self) -> bool {
        self.parts.is_empty()
    }

    pub fn add(&mut self, part: BytePart) {
        self.parts.push(part)
    }

    pub fn append(&mut self, vec: &mut Vec<BytePart>) {
        self.parts.append(vec)
    }

    pub fn matches<'a>(&'a self, bytes: &'a [u8]) -> PatternMatches<'a> {
        if self.is_empty() {
            panic!("Cannot match on empty pattern!")
        }
        PatternMatches{ source: bytes, pattern: self, position: 0 }
    }

    pub fn is_match(&self, bytes: &[u8]) -> PatternMatchResult {
        if self.len() != bytes.len() {
            Err(PatternError::from_kind(PatternErrorKind::SizeMismatch))
        }
        else {
            Ok(self.parts.iter().zip(bytes.iter()).all(|pair| pair.0.is_match(*pair.1)))
        }
    }

    pub fn apply(&self, bytes: &mut [u8]) -> PatternApplyResult {
        if self.len() != bytes.len() {
            Err(PatternError::from_kind(PatternErrorKind::SizeMismatch))
        }
        else {
            self.parts.iter().zip(bytes.iter_mut()).for_each(|pair| *(pair.1) = pair.0.apply(*pair.1));
            Ok(())
        }
    }
}

impl <'a> Iterator for PatternMatches<'a> {
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

#[cfg(test)]
mod pattern_tests {
    use super::*;

    #[test]
    fn pattern_parse_empty() {
        assert!(Pattern::parse("").unwrap().is_empty());
        assert_eq!(0, Pattern::parse("").unwrap().len());
    }

    #[test]
    fn pattern_matches() {
        let p = Pattern::parse("5? A2 ?? ?8").unwrap();
        let b = &[0x33, 0x5F, 0xA2, 0xCC, 0xD8, 0x23, 0x00, 0x54, 0xA2, 0xDD, 0x28, 0x30];

        assert_eq!(&[1, 7], p.matches(b).collect::<Vec<_>>().as_slice());
    }
}
