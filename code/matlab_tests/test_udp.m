%% MATLAB Unity3D UDP Connection
% Sandra Fang
% 2016


% Create UDP connection - need one port for reading and another for receiving 
clear all;
clc;
t = udp('10.225.42.154',9000,'LocalPort', 9001);
fopen(t);
disp(t.status);
i = 0;
connection_bool = 0;

while(connection_bool == 0)
   % Send messages to localhost's port
   %fwrite(t, (65+i):73); 
   % Read incoming messages from LocalPort
   A = fread(t, 10);
   B = fscanf(t);
   %disp(A);
   disp(B);
   if(contains(B,'quit'))
       disp("closing down communication");
       connection_bool = 1;
       fclose(t);
   end
end


%remember to close the connection using fclose(t)