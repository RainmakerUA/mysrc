extern crate binpatcher;

use std::fs::File;
use std::io::{ prelude::*, SeekFrom };
use std::time::{/*Duration,*/ Instant};

use binpatcher::model::{ BytePart, Pattern };

fn main() {
    let bp1 = BytePart::full(0xF2);
    let bp2 = BytePart::low(0xF);

    println!("{}", bp1);
    println!("{:?}", bp2);

    println!("Any byte: {}", BytePart::any());
    println!("Low byte part: {}", BytePart::low(0xC).unwrap());
    println!("High byte part: {}", BytePart::high(0xA).unwrap());
    println!("Full byte: {}", BytePart::full(0x4A));

    println!("BytePart: {:?}", BytePart::low(0xFF));

    println!("BytePart: {}", parse_bytepart("?1"));
    println!("BytePart: {}", parse_bytepart("??"));
    println!("BytePart: {}", parse_bytepart("F1"));
    println!("BytePart: {}", parse_bytepart("C?"));
    //println!("BytePart: {}", parse_bytepart("X1")); // panic!

    let p = Pattern::parse("7E ??????04 25 17 6A 58").unwrap();
    let b = &mut [0x7E, 0x23, 0x65, 0x99, 0x04, 0x25, 0x17, 0x6A, 0x58];
    println!("Is Match: {:?}", p.is_match(b));

    let patch = Pattern::parse("?? ???????? 2B ?? ?? ??").unwrap();
    match patch.apply(b) {
        Ok(_) => println!("Applied: {:?}", b),
        Err(error) => println!("{:?}", error)
    }

    match Pattern::parse("?? ???????? 2B ?? ? ??") {
        Ok(p) => println!("{:?}", p),
        Err(e) => println!("Error: {}", e)
    }

    let p = Pattern::parse("5? A2 ?? ?8").unwrap();
    let b = &[0x33, 0x5F, 0xA2, 0xCC, 0xD8, 0x23, 0x00, 0x54, 0xA2, 0xDD, 0x28, 0x30];

    for pos in p.matches(b) {
        println!("Match found @ 0x{:02X}", pos)
    }

    println!("Loading a file...");
    
    let mut f = File::open(r"e:\Torrents\x.bin").expect("Cannot open file!");
    let len = f.seek(SeekFrom::End(0)).expect("Cannot seek in the file!") as usize;
    f.seek(SeekFrom::Start(0)).expect("Cannot seek in the file!");
    println!("File size = {} bytes", len);

    let mut v = Vec::with_capacity(len + 16);
    let p = Pattern::parse("5? A2 ?? ?8 FF E?").unwrap();
    let start = Instant::now();
    let len = f.read_to_end(&mut v).expect("Cannot read file!");
    println!("File read in {:?} : {} bytes", start.elapsed(), len);
    println!("Scanning a file...");

    //let len = v.len();
    for pos in p.matches(&v[..]) {
        println!("Match found @ 0x{:02X} : {:02}% | Elapsed: {:?}", pos, 100 * pos / len, start.elapsed());
    }
    println!("Scan finished after {:?}", start.elapsed());
}

fn parse_bytepart(s: &str) -> BytePart {
    match s.parse::<BytePart>() {
        Ok(bp) => bp,
        Err(err) => panic!("Cannot parse BytePart '{}': {}", s, err)
    }
}
