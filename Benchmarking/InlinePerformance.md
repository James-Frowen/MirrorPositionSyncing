1,000,000,000 iterations

Test function
```cs
public void Write(byte a)
{
    buffer[next] = a;
    next++;
    if (next >= buffer.Length) { next = 0; }
}
```
was tested with buffer/next as public and private fields.


Fields  |   inline   | Type                           | Time(ms) 
--------|------------|--------------------------------|----------  
private | no         | class                          |  2730    
public  | no         | class                          |  2747    
private | no         | interface                      |  2887    
public  | no         | interface                      |  2864    
private | no         | interface                      |  2809    
public  | no         | interface                      |  2712
private | aggressive | class                          |  1955    
public  | aggressive | class                          |  1957    
public  | manual     | class                          |  2095    
private | aggressive | interface                      |  2887    
public  | aggressive | interface                      |  2861    
private | aggressive | interface (marked with inline) |  2922    
public  | aggressive | interface (marked with inline) |  2938    

## Summary
- Aggressive and Manual about the same
- public/private field doesn't effect
- Aggressive does not work with interfaces