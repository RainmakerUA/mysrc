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

impl Pattern {
    pub fn parse(text: &str) -> Result<Self, ParseBytePartError> {
        let mut iter = text.bytes().filter(|b| !b.is_ascii_whitespace());
        let mut parts = Vec::new();

        loop {
            let first = iter.next();
            let second = iter.next();

            if let Some(first) = first {
                if let Some(second) = second {
                    parts.push(BytePart::parse(first, second)?);
                    continue;
                }
                else {
                    return Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength));
                }
            }
            break;
        }
        Ok(Pattern { parts })
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

#[cfg(test)]
mod pattern_tests {
    use super::*;

    #[test]
    fn pattern_parse_empty() {
        assert!(Pattern::parse("").unwrap().is_empty());
        assert_eq!(0, Pattern::parse("").unwrap().len());
    }
}
