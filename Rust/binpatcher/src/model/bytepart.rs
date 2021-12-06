use std::fmt;
use std::str::FromStr;
use super::{ ParseBytePartError, BytePartErrorKind };

const QUESTION: u8 = b'?';
const TEN: u8 = 0x0A;
const NUM_0: u8 = b'0';
const NUM_9: u8 = b'9';
const UPPER_A: u8 = b'A';
const UPPER_F: u8 = b'F';
const LOWER_A: u8 = b'a';
const LOWER_F: u8 = b'f';

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct BytePart {
    value: BytePartValue
}

#[derive(Debug, Clone, PartialEq, Eq)]
enum BytePartValue {
    Any,
    Low(u8),
    High(u8),
    Full(u8)
}

impl BytePart {
    pub fn any() -> Self {
        BytePart { value: BytePartValue::Any }
    }

    pub fn low(byte: u8) -> Self {
        BytePart { value: BytePartValue::Low(low_nibble(byte)) }
    }

    pub fn high(byte: u8) -> Self {
       BytePart { value: BytePartValue::High(low_nibble(byte)) }
    }

    pub fn full(byte: u8) -> Self {
        BytePart { value: BytePartValue::Full(byte) }
    }

    pub fn parse(high: u8, low: u8) -> Result<BytePart, ParseBytePartError> {
        Ok(BytePart { value: BytePartValue::from_parts(try_get_nibble(Some(high))?, try_get_nibble(Some(low))?) })
    }

    pub fn is_match(&self, byte: u8) -> bool {
        match self.value {
            BytePartValue::Any => true,
            BytePartValue::Low(part) => (byte & 0x0F) == part,
            BytePartValue::High(part) => (byte & 0xF0) == part << 4,
            BytePartValue::Full(val) => byte == val
        }
    }

    pub fn apply(&self, byte: u8) -> u8 {
        match self.value {
            BytePartValue::Any => byte,
            BytePartValue::Low(part) => (byte & 0xF0) | part,
            BytePartValue::High(part) => (byte & 0x0F) | (part << 4),
            BytePartValue::Full(val) => val
        }
    }
}

impl fmt::Display for BytePart {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        let mut out: Vec<u8> = Vec::new();
        match self.value {
            BytePartValue::Any => {
                    out.push(QUESTION);
                    out.push(QUESTION);
                },
            BytePartValue::Low(part) => {
                    out.push(QUESTION);
                    out.push(nibble_to_hexchar(part));
                },
            BytePartValue::High(part) => {
                    out.push(nibble_to_hexchar(part));
                    out.push(QUESTION);
                },
            BytePartValue::Full(byte) => {
                    out.push(nibble_to_hexchar((byte & 0xF0) >> 4));
                    out.push(nibble_to_hexchar(byte & 0x0F));
                }
            
        };
        String::from_utf8(out).unwrap().fmt(f)
    }
}

impl FromStr for BytePart {
    type Err = ParseBytePartError;
    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let mut bytes = s.bytes();
        let high = try_get_nibble(bytes.next())?;
        let low = try_get_nibble(bytes.next())?;
        if bytes.next().is_some() {
            Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength))
        }
        else {
            Ok(BytePart { value: BytePartValue::from_parts(high, low) })
        }
    }
}

impl BytePartValue {
    fn from_parts(high: Option<u8>, low: Option<u8>) -> Self {
        if let Some(h) = high {
            if let Some(l) = low {
                BytePartValue::Full(h << 4 | l)
            }
            else {
                BytePartValue::High(h)
            }
        }
        else if let Some(l) = low {
                BytePartValue::Low(l)
        }
        else {
            BytePartValue::Any
        }
    }
}

#[inline]
fn low_nibble(byte: u8) -> u8 {
    byte & 0xF0
}

fn nibble_to_hexchar(nibble: u8) -> u8 {
    if nibble < TEN {
        nibble + NUM_0
    }
    else {
        nibble - TEN + UPPER_A
    }
}

fn hexchar_to_nibble(hex: u8) -> Result<u8, ParseBytePartError> {
    match hex {
        NUM_0 ..= NUM_9 => Ok(hex - NUM_0),
        UPPER_A ..= UPPER_F => Ok(hex - UPPER_A + TEN),
        LOWER_A ..= LOWER_F => Ok(hex - LOWER_A + TEN),
        _ => Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(hex)))
    }
}

fn try_get_nibble(c: Option<u8>) -> Result<Option<u8>, ParseBytePartError> {
    match c {
        None => Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength)),
        Some(QUESTION) => Ok(None),
        Some(chr) => hexchar_to_nibble(chr).map(Some)
    }
}

#[cfg(test)]
mod bytepart_tests {
    use super::*;

    #[test]
    fn bytepart_factory_any() {
        assert_eq!(BytePartValue::Any, BytePart::any().value);
    }

    #[test]
    fn bytepart_factory_low_ok() {
        let byte = 0x0D_u8;
        assert_eq!(BytePartValue::Low(byte), BytePart::low(byte).value);
    }

    #[test]
    fn bytepart_factory_low_downcast() {
        let byte = 0xD2_u8;
        assert_eq!(BytePartValue::Low(low_nibble(byte)), BytePart::low(byte).value);
    }

    #[test]
    fn bytepart_factory_high_ok() {
        let byte = 0x0B_u8;
        assert_eq!(BytePartValue::High(byte), BytePart::high(byte).value);
    }

    #[test]
    fn bytepart_factory_high_downcast() {
        let byte = 0x2D_u8;
        assert_eq!(BytePartValue::High(low_nibble(byte)), BytePart::high(byte).value);
    }

    #[test]
    fn bytepart_factory_full() {
        let byte = 0x9D_u8;
        assert_eq!(BytePartValue::Full(byte), BytePart::full(byte).value);
    }

    #[test]
    fn bytepart_factory_parse_ok_any() {
        assert_eq!(BytePartValue::Any, BytePart::parse(b'?', b'?').unwrap().value);
    }

    #[test]
    fn bytepart_factory_parse_ok_low() {
        assert_eq!(BytePartValue::Low(0x8), BytePart::parse(b'?', b'8').unwrap().value);
    }

    #[test]
    fn bytepart_factory_parse_ok_high() {
        assert_eq!(BytePartValue::High(0x0), BytePart::parse(b'0', b'?').unwrap().value);
    }

    #[test]
    fn bytepart_factory_parse_ok_full() {
        assert_eq!(BytePartValue::Full(0x54), BytePart::parse(b'5', b'4').unwrap().value);
    }

    #[test]
    fn bytepart_factory_parse_fail_low() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'Q'))), BytePart::parse(b'?', b'Q'));
    }

    #[test]
    fn bytepart_factory_parse_fail_high() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'Z'))), BytePart::parse(b'Z', b'?'));
    }

    #[test]
    fn bytepart_is_match_any() {
        assert!(BytePart { value: BytePartValue::Any }.is_match(0x55));
    }

    #[test]
    fn bytepart_is_match_low_true() {
        assert!(BytePart { value: BytePartValue::Low(0xA) }.is_match(0x8A));
    }

    #[test]
    fn bytepart_is_match_low_false() {
        assert!(!BytePart { value: BytePartValue::Low(0xD) }.is_match(0x85));
    }

    #[test]
    fn bytepart_is_match_high_true() {
        assert!(BytePart { value: BytePartValue::High(0xF) }.is_match(0xF5));
    }

    #[test]
    fn bytepart_is_match_high_false() {
        assert!(!BytePart { value: BytePartValue::High(0x0) }.is_match(0x50));
    }

    #[test]
    fn bytepart_is_match_full_true() {
        assert!(BytePart { value: BytePartValue::Full(0x71) }.is_match(0x71));
    }

    #[test]
    fn bytepart_is_match_full_false() {
        assert!(!BytePart { value: BytePartValue::Full(0x12) }.is_match(0x13));
    }

    #[test]
    fn bytepart_apply_any() {
        assert_eq!(0x32, BytePart { value: BytePartValue::Any }.apply(0x32));
    }

    #[test]
    fn bytepart_apply_low() {
        assert_eq!(0xBB, BytePart { value: BytePartValue::Low(0xB) }.apply(0xB2));
    }

    #[test]
    fn bytepart_apply_high() {
        assert_eq!(0xA0, BytePart { value: BytePartValue::High(0xA) }.apply(0x30));
    }

    #[test]
    fn bytepart_apply_full() {
        assert_eq!(0x56, BytePart { value: BytePartValue::Full(0x56) }.apply(0x22));
    }

    #[test]
    fn bytepart_format_any() {
        assert_eq!("??", &format!("{}", BytePart{ value: BytePartValue::Any }));
    }

    #[test]
    fn bytepart_format_low() {
        assert_eq!("?9", &format!("{}", BytePart{ value: BytePartValue::Low(0x9) }));
    }

    #[test]
    fn bytepart_format_high() {
        assert_eq!("0?", &format!("{}", BytePart{ value: BytePartValue::High(0x0) }));
    }

    #[test]
    fn bytepart_format_full() {
        assert_eq!("A3", &format!("{}", BytePart{ value: BytePartValue::Full(0xA3) }));
    }

    #[test]
    fn bytepart_from_str_ok_any() {
        assert_eq!(Ok(BytePart{ value: BytePartValue::Any }), "??".parse());
    }

    #[test]
    fn bytepart_from_str_ok_low() {
        assert_eq!(Ok(BytePart{ value: BytePartValue::Low(0xD) }), "?D".parse());
    }

    #[test]
    fn bytepart_from_str_ok_high() {
        assert_eq!(Ok(BytePart{ value: BytePartValue::High(0x5) }), "5?".parse());
    }

    #[test]
    fn bytepart_from_str_ok_full() {
        assert_eq!(Ok(BytePart{ value: BytePartValue::Full(0x6C) }), "6C".parse());
    }

    #[test]
    fn bytepart_from_str_fail_invalid_low() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'Z'))), "?Z".parse::<BytePart>());
    }

    #[test]
    fn bytepart_from_str_fail_invalid_high() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'!'))), "!D".parse::<BytePart>());
    }

    #[test]
    fn bytepart_from_str_fail_invalid_length_1() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength)), "?".parse::<BytePart>());
    }

    #[test]
    fn bytepart_from_str_fail_invalid_length_3() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength)), "235".parse::<BytePart>());
    }

    #[test]
    fn bytepartvalue_from_parts_any() {
        assert_eq!(BytePartValue::Any, BytePartValue::from_parts(None, None));
    }

    #[test]
    fn bytepartvalue_from_parts_low() {
        assert_eq!(BytePartValue::Low(0x7), BytePartValue::from_parts(None, Some(0x7)));
    }

    #[test]
    fn bytepartvalue_from_parts_high() {
        assert_eq!(BytePartValue::High(0x4), BytePartValue::from_parts(Some(0x4), None));
    }

    #[test]
    fn bytepartvalue_from_parts_full() {
        assert_eq!(BytePartValue::Full(0x98), BytePartValue::from_parts(Some(0x9), Some(0x8)));
    }

    #[test]
    fn parsebyteparterror_description_invalid_char() {
        let descr = ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'G')).description();
        assert_eq!("'G'!", &descr[descr.len() - 4 ..]);
    }

    #[test]
    fn parsebyteparterror_description_wrong_length() {
        let descr = ParseBytePartError::from_kind(BytePartErrorKind::WrongLength).description();
        assert_eq!("Wrong length!", &descr);
    }

    #[test]
    fn parsebyteparterror_fmt_invalid_char() {
        let descr = format!("{}", ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'G')));
        assert_eq!("'G'!", &descr[descr.len() - 4 ..]);
    }

    #[test]
    fn parsebyteparterror_fmt_wrong_length() {
        let descr = format!("{}", ParseBytePartError::from_kind(BytePartErrorKind::WrongLength));
        assert_eq!("Wrong length!", &descr);
    }

    #[test]
    fn low_nibble_result1() {
        assert_eq!(0x02, low_nibble(0x02));
    }

    #[test]
    fn low_nibble_result2() {
        assert_eq!(0x00, low_nibble(0x40));
    }

    #[test]
    fn low_nibble_result3() {
        assert_eq!(0x02, low_nibble(0xD2));
    }

    #[test]
    fn nibble_to_hexchar_num() {
        assert_eq!(b'8', nibble_to_hexchar(0x8));
    }

    #[test]
    fn nibble_to_hexchar_letter() {
        assert_eq!(b'D', nibble_to_hexchar(0xD));
    }

    #[test]
    fn hexchar_to_nibble_ok_num() {
        assert_eq!(Ok(0x4), hexchar_to_nibble(b'4'));
    }

    #[test]
    fn hexchar_to_nibble_ok_upper_letter() {
        assert_eq!(Ok(0xC), hexchar_to_nibble(b'C'));
    }

    #[test]
    fn hexchar_to_nibble_ok_lower_letter() {
        assert_eq!(Ok(0xF), hexchar_to_nibble(b'f'));
    }

    #[test]
    fn hexchar_to_nibble_fail1() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'S'))), hexchar_to_nibble(b'S'));
    }

    #[test]
    fn hexchar_to_nibble_fail2() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'@'))), hexchar_to_nibble(b'@'));
    }

    #[test]
    fn try_get_nibble_ok_num() {
        assert_eq!(Ok(Some(0x2)), try_get_nibble(Some(b'2')));
    }

    #[test]
    fn try_get_nibble_ok_lower_letter() {
        assert_eq!(Ok(Some(0xA)), try_get_nibble(Some(b'a')));
    }

    #[test]
    fn try_get_nibble_ok_upper_letter() {
        assert_eq!(Ok(Some(0xE)), try_get_nibble(Some(b'E')));
    }

    #[test]
    fn try_get_nibble_ok_question() {
        assert_eq!(Ok(Some(0x2)), try_get_nibble(Some(b'2')));
    }

    #[test]
    fn try_get_nibble_fail_length() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::WrongLength)), try_get_nibble(None));
    }

    #[test]
    fn try_get_nibble_fail_char1() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'w'))), try_get_nibble(Some(b'w')));
    }

    #[test]
    fn try_get_nibble_fail_char2() {
        assert_eq!(Err(ParseBytePartError::from_kind(BytePartErrorKind::InvalidChar(b'K'))), try_get_nibble(Some(b'K')));
    }
}
