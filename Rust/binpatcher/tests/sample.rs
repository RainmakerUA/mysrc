//extern crate binpatcher;

use binpatcher::model::BytePart;

mod common;

#[test]
fn sample_test_bytepart() {
    common::setup();
    assert_eq!("??".parse::<BytePart>().unwrap(), BytePart::any());
}
