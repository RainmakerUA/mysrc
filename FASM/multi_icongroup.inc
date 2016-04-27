macro multi_icon group,[label,icon_file,num] 
 { common local count 
    count = 0 
   forward local data,size,position 
    label dd RVA data,size,0,0 
    virtual at 0 
     file icon_file:6+(num*16),16 
     load size dword from 8 
     load position dword from 12 
    end virtual 
    data file icon_file:position,size 
    count = count+1 
   common local header 
    align 4 
    group dd RVA header,6+count*14,0,0 
    header dw 0,1,count 
   forward 
    file icon_file:6+(num*16),12 
    dw label#.resid 
   common 
    align 4 }